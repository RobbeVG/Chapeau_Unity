using System;
using UnityEngine;

namespace Seacore
{
    public class PickupDragController : MonoBehaviour
    {
        private Camera _mainCamera;
        private GameObject _selectedObject;
        private Vector3 _offset;
        private Plane _groundPlane;
        private Rigidbody _selectedRigidbody;

        public event Action<GameObject> ObjectPickedUp;
        public event Action<GameObject> ObjectDropped;

        [SerializeField]
        private LayerMask _pickupLayerMask;
        [SerializeField]
        private float snapSpeed = 10f; // Speed at which the object will snap to the mouse position

        private void Start()
        {
            _mainCamera = Camera.main;
            _groundPlane = new Plane(Vector3.up, new Vector3(0, 0.25f, 0)); // Assuming ground is at y=0.25 and is flat
        }

        private void Update()
        {
            HandlePickup();
            HandleDrop();
        }

        private void FixedUpdate()
        {
            HandleDrag();
        }

        private void HandlePickup()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _pickupLayerMask))
                {
                    _selectedObject = hit.transform.gameObject;
                    _selectedRigidbody = _selectedObject.GetComponent<Rigidbody>();
                    if (_selectedRigidbody != null)
                    {
                        _offset = _selectedObject.transform.position - GetMouseWorldPosition();
                        ObjectPickedUp?.Invoke(_selectedObject);
                    }
                }
            }
        }

        private void HandleDrag()
        {
            if (_selectedObject != null && Input.GetMouseButton(0))
            {
                Vector3 targetPosition = GetMouseWorldPosition() + _offset;
                _selectedObject.transform.position = Vector3.Lerp(_selectedObject.transform.position, targetPosition, snapSpeed * Time.fixedDeltaTime);
            }
        }

        private void HandleDrop()
        {
            if (_selectedObject != null && Input.GetMouseButtonUp(0))
            {
                ObjectDropped?.Invoke(_selectedObject);

                //_selectedObject.transform.position
                // Keep the object kinematic after dropping
                _selectedObject = null;
                _selectedRigidbody = null;
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (_groundPlane.Raycast(ray, out float enter))
            {
                return ray.GetPoint(enter);
            }
            return Vector3.zero;
        }
    }
}
