using System;
using UnityEngine;

namespace Seacore
{
    /// <summary>
    /// A component script that allows the user to select an object on the screen,
    /// depending on their layerMask.
    /// </summary>
    /// <remarks>
    /// Does not handle the input; this should be handled separately.
    /// </remarks>
    public class ObjectSelector  : MonoBehaviour
    {
        private Camera _mainCamera;

        [SerializeField]
        [Tooltip("Define a layerMask to select the object in")]
        private LayerMask _pickupLayerMask;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        /// <summary>
        /// Try to select object from position on screen.
        /// </summary>
        /// <param name="mousePosition"><inheritdoc cref="Input.mousePosition"/></param>
        public GameObject SelectObjectFromScreen(Vector3 mousePosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _pickupLayerMask))
            {
                return hit.transform.gameObject;
            }
            return null;
        }
    }
}
