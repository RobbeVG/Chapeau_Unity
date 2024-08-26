using System;
using UnityEngine;
//using UnityEngine.Input

namespace Seacore
{
    /// <summary>
    /// A component script that allows the user to select an object on the screen,
    /// depending on their layerMask.
    /// When enabled the script will update every frame so you can retrieve the hovered object.
    /// </summary>
    /// <remarks>
    /// Does not handle the input; this should be handled separately.
    /// </remarks>
    public class ObjectSelector  : MonoBehaviour
    {
        [SerializeField]
        private Camera _mainCamera;
        private GameObject _hoveredObject;

        public Action<GameObject> OnHover;
        public Action<GameObject> OnExit;

        [SerializeField]
        [Tooltip("Define a layerMask to select the object in")]
        private LayerMask _pickupLayerMask;

        private void Update()
        {
            GameObject hoveredObject = GetObjectFromScreen(Input.mousePosition);
            if (hoveredObject != null)
            {
                if (_hoveredObject != null)
                OnHover?.Invoke(_hoveredObject);
            }
            //else ()
        }

        /// <summary>
        /// Try to select object from position on screen.
        /// </summary>
        /// <param name="mousePosition"><inheritdoc cref="Input.mousePosition"/></param>
        private GameObject GetObjectFromScreen(Vector3 mousePosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _pickupLayerMask))
            {
                return hit.transform.gameObject;
            }
            return null;
        }

        public GameObject SelectObject()
        {
            return enabled ? _hoveredObject : GetObjectFromScreen(Input.mousePosition);
        }
    }
}
