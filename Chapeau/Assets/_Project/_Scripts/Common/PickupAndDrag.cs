using System;
using System.Collections;
using UnityEngine;

namespace Seacore.Common
{
    /// <summary>
    /// A component script that allows the user to pick up objects and drag them around on the screen.
    /// </summary>
    /// <remarks>
    /// Does not handle the input this should be handled seperatly.
    /// </remarks>
    public class PickupAndDrag : MonoBehaviour
    {
        public GameObject SelectedObject { get; private set; }
        private Vector3 _offset;
        private float _originalHeightvalue;

        [SerializeField]
        private float _snapSpeed = 10f; // Speed at which the object will snap to the mouse position
        [SerializeField]
        private float _dropDuration = 0.5f;

        //Internal
        private const float _pickupHeightOffset = 0.4f;

        private void FixedUpdate()
        {
            HandleDrag();
        }

        /// <summary>
        /// Picking up an object and offesting it to the current mouse position.
        /// </summary>
        /// <param name="gameObject"></param>
        public void HandlePickup(GameObject gameObject)
        {            
            SelectedObject = gameObject;
            _originalHeightvalue = SelectedObject.transform.position.y;
            _offset = SelectedObject.transform.position - GetMouseWorldPosition();
            _offset.y += _pickupHeightOffset;
        }

        /// <summary>
        /// Drags the object around
        /// </summary>
        public void HandleDrag()
        {
            if (SelectedObject != null)
            {
                Vector3 targetPosition = GetMouseWorldPosition() + _offset;
                SelectedObject.transform.position = Vector3.Lerp(SelectedObject.transform.position, targetPosition, _snapSpeed * Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Drops the selected object to the ground
        /// </summary>
        public GameObject HandleDrop()
        {
            GameObject droppedGameObject = SelectedObject;
            if (SelectedObject != null)
            {
                StartCoroutine(DropObjectToHeight(SelectedObject));
                SelectedObject = null;
            }
            return droppedGameObject;
        }

        /// <summary>
        /// A smooth dropdown of the selected object
        /// </summary>
        /// <remarks>This function does not set the selevtedOBject to null </remarks>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public IEnumerator DropObjectToHeight(GameObject gameObject)
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane _groundPlane = new Plane(Vector3.up, new Vector3(0, _originalHeightvalue + _pickupHeightOffset, 0));

            if (_groundPlane.Raycast(ray, out float enter))
            {
                return ray.GetPoint(enter);
            }
            return Vector3.zero;
        }
    }
}
