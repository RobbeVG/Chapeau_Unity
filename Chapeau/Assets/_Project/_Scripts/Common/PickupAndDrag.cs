using DG.Tweening;
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
        public GameObject SelectedObject { get; private set; } = null;

        [SerializeField] 
        private float _snapSpeed = 10f;
        [SerializeField] 
        private float _dropDuration = 0.5f;
        [SerializeField] 
        private float _pickupHeightOffset = 0.4f;

        private float _originalHeightvalue;

        // Called by InputManager when an object is picked up
        public void HandlePickup(GameObject gameObject)
        {
            SelectedObject = gameObject;
            _originalHeightvalue = SelectedObject.transform.position.y;
        }

        // Called by InputManager every frame with the desired world position
        public void HandleDrag(Vector3 targetWorldPosition)
        {
            if (SelectedObject != null)
            {
                Vector3 target = targetWorldPosition;
                target.y = _pickupHeightOffset + _originalHeightvalue;
                SelectedObject.transform.position = Vector3.Lerp(SelectedObject.transform.position, target, _snapSpeed * Time.deltaTime);
            }
        }

        public GameObject HandleDrop()
        {
            GameObject droppedGameObject = SelectedObject;
            if (SelectedObject != null)
            {
                SelectedObject.transform.DOMoveY(_originalHeightvalue, _dropDuration);
                SelectedObject = null;
            }
            return droppedGameObject;
        }
    }
}
