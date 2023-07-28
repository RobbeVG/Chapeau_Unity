using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class GrabController : MonoBehaviour
    {
        public GameObject Selected { get; private set; } = null;
        private Collider colliderSelected;
        private Rigidbody rigidbodySelected;
        private float originalYPosition;
        
        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private float liftYOffset = 0.25f;

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) //Pick up
            {
                if (!Selected)
                {
                    RaycastHit hit = CastRay();
                    if (hit.collider != null)
                    {
                        Selected = hit.collider.gameObject;
                        colliderSelected = hit.collider;
                        rigidbodySelected = colliderSelected.attachedRigidbody;
                        originalYPosition = Selected.transform.position.y;

                        //Diable collider and if attached the rigidbody to use gravity
                        colliderSelected.enabled = false;
                        rigidbodySelected.useGravity = false;

                        

                        Cursor.visible = false;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) // Drop
            {
                if (Selected)
                {
                    MoveSelected();

                    colliderSelected.enabled = true;
                    rigidbodySelected.useGravity = true;

                    Selected = null;
                    Cursor.visible = true;
                }
                return;
            }

            //When selected object
            if (Selected != null)
            {
                MoveSelected(liftYOffset);
            }
        }

        private void MoveSelected(float yOffset = 0.0f)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(Selected.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            Selected.transform.position = new Vector3(worldPosition.x, originalYPosition + yOffset, worldPosition.z);
        }

        private RaycastHit CastRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask.value); // All different objects are not raycastable (See: layers)
            return hit;
        }
    }
}
