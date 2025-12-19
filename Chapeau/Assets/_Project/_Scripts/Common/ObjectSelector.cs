using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore.Common
{
    /// <summary>
    /// A component script that allows the user to select an object on the screen,
    /// depending on their layerMask.
    /// When enabled the script will update every frame so you can retrieve the hovered object.
    /// </summary>
    /// <remarks>
    /// You can manually update the script on every pointer moved
    /// Does not handle the input; this should be handled separately.
    /// </remarks>
    public class ObjectSelector  : MonoBehaviour
    {
        [SerializeField]
        private Camera _mainCamera;

        [SerializeField]
        [ReadOnly]
        private GameObject _hoveredObject = null;
        /// <summary>
        /// Get the hover object
        /// </summary>
        public GameObject ObjectOnCursor => _hoveredObject;

        [field: SerializeField]
        [Tooltip("Define a layerMask to select the object in")]
        public LayerMask PickupLayerMask { get; private set; }

        /// <summary>
        /// Only call this method when you have disabled the gameObject
        /// </summary>
        public void SetObjectFromPosition(Vector2 pointerPosition)
        {
            _hoveredObject = GetObjectFromScreen(pointerPosition);
        }

        /// <summary>
        /// Try to select object from position on screen.
        /// </summary>
        /// <param name="mousePosition"><inheritdoc cref="Input.mousePosition"/></param>
        private GameObject GetObjectFromScreen(Vector2 mousePosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, PickupLayerMask))
            {
                return hit.transform.gameObject;
            }
            return null;
        }

    }
}
