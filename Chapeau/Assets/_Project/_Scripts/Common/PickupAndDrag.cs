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
        [SerializeField] private float _snapSpeed = 10f;
        [SerializeField] private float _dropDuration = 0.5f;
        [SerializeField] private float _pickupHeightOffset = 0.4f;
        public float PickupHeightOffset { get { return _pickupHeightOffset; } }


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
                target.y = _pickupHeightOffset;
                SelectedObject.transform.position = Vector3.Lerp( SelectedObject.transform.position, target, _snapSpeed * Time.deltaTime);
            }
        }

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

        private IEnumerator DropObjectToHeight(GameObject gameObject)
        {
            float timeElapsed = 0;
            float startY = gameObject.transform.position.y;
            while (timeElapsed < _dropDuration)
            {
                float t = timeElapsed / _dropDuration;
                float newY = Mathf.Lerp(startY, _originalHeightvalue, t);
                timeElapsed += Time.deltaTime;
                gameObject.transform.position = new Vector3(
                    gameObject.transform.position.x, newY, gameObject.transform.position.z);
                yield return null;
            }
            gameObject.transform.position = new Vector3(
                gameObject.transform.position.x, _originalHeightvalue, gameObject.transform.position.z);
        }
    }
}
