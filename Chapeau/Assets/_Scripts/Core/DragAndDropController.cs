using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class DragAndDropController : MonoBehaviour
    {
        public GameObject Selected { get; private set; } = null;
        private Rigidbody rigidbodySelected;
        private float originalYPosition;

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private float liftYOffset = 0.25f;
        [SerializeField]
        private float dragForce = 10.0f;

        public event Action<GameObject> OnGameObjectDropped;
        public event Action<GameObject> OnGameObjectSelected;

        void Update()
        {
            if (Selected)
            {
                if (Input.GetMouseButtonUp(0)) // Drop
                {
                    MoveSelected();
                    rigidbodySelected.useGravity = true;
                    rigidbodySelected.isKinematic = true;
                    rigidbodySelected.constraints = 0;

                    OnGameObjectDropped?.Invoke(Selected);
                    Selected = null;
                    Cursor.visible = true;
                }
                else
                {
                    MoveSelected(liftYOffset);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0)) //Pick up
                {
                    RaycastHit hit = CastRay();
                    if (hit.collider != null)
                    {
                        Selected = hit.collider.gameObject;
                        rigidbodySelected = hit.collider.attachedRigidbody;
                        originalYPosition = Selected.transform.position.y;

                        //Diable collider and if attached the rigidbody to use gravity
                        rigidbodySelected.useGravity = false;
                        rigidbodySelected.isKinematic = false; //colliding with other dice

                        rigidbodySelected.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation /*| RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ*/;
                        OnGameObjectSelected?.Invoke(Selected);

                        //_currentSpring = Selected.AddComponent<SpringJoint>();
                        //_currentSpring.autoConfigureConnectedAnchor = false;
                        Cursor.visible = false;
                    }
                }
            }
                ////NEW
                //Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(Selected.transform.position).z);
                //_currentSpring.connectedAnchor = Selected.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void MoveSelected(float yOffset = 0.0f)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(Selected.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            rigidbodySelected.velocity = (worldPosition - Selected.transform.position);
            rigidbodySelected.velocity.Scale(new Vector3(dragForce, 0.0f, dragForce));
            //Selected.transform.position = new Vector3(worldPosition.x, originalYPosition + yOffset, worldPosition.z);
        }

        private RaycastHit CastRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask.value); // All different objects are not raycastable (See: layers)
            return hit;
        }
    }
}
