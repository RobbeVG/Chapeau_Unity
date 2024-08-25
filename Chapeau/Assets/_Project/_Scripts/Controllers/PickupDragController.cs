using System;
using System.Collections;
using UnityEngine;

namespace Seacore
{
    /// <summary>
    /// A controller script that allows the user to pick up objects
    /// </summary>
    public class PickupDragController : MonoBehaviour
    {
        private Camera _mainCamera;
        private GameObject _selectedObject;
        private Vector3 _offset;
        private float _originalHeightvalue;

        public event Action<GameObject> ObjectPickedUp;
        public event Action<GameObject> ObjectDropped;

        [SerializeField]
        private LayerMask _pickupLayerMask;
        [SerializeField]
        private float _snapSpeed = 10f; // Speed at which the object will snap to the mouse position
        [SerializeField]
        private float _dropDuration = 0.5f;

        //Internal
        private const float _pickupHeightOffset = 0.4f;

        private void Start()
        {
            _mainCamera = Camera.main;
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
                    _originalHeightvalue = _selectedObject.transform.position.y;
                    _offset = _selectedObject.transform.position - GetMouseWorldPosition();
                    _offset.y += _pickupHeightOffset;
                    ObjectPickedUp?.Invoke(_selectedObject);
                }
            }
        }

        private void HandleDrag()
        {
            if (_selectedObject != null && Input.GetMouseButton(0))
            {
                Vector3 targetPosition = GetMouseWorldPosition() + _offset;
                _selectedObject.transform.position = Vector3.Lerp(_selectedObject.transform.position, targetPosition, _snapSpeed * Time.fixedDeltaTime);
            }
        }

        private void HandleDrop()
        {
            if (_selectedObject != null && Input.GetMouseButtonUp(0))
            {
                ObjectDropped?.Invoke(_selectedObject);
                StartCoroutine(DropObjectToHeight(_selectedObject));
                _selectedObject = null;
            }
        }

        /// <summary>
        /// A smooth dropdown of the selected object
        /// </summary>
        /// <remarks>This function does not set the selevtedOBject to null </remarks>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        IEnumerator DropObjectToHeight(GameObject gameObject)
        {
            float timeElapsed = 0;
            while (timeElapsed < _dropDuration)
            {
                float t = timeElapsed / _dropDuration;
                float newHeightPos = Mathf.SmoothStep(gameObject.transform.position.y, _originalHeightvalue, t);
                timeElapsed += Time.deltaTime;

                gameObject.transform.position = new Vector3(gameObject.transform.position.x, newHeightPos, gameObject.transform.position.z);
                yield return null;
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane _groundPlane = new Plane(Vector3.up, new Vector3(0, _originalHeightvalue + _pickupHeightOffset, 0));

            if (_groundPlane.Raycast(ray, out float enter))
            {
                return ray.GetPoint(enter);
            }
            return Vector3.zero;
        }
    }
}
