using Seacore.Common;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

namespace Seacore.Game
{
    /// <summary>
    /// A specified player controller that handles the input from the input System.
    /// </summary>
    /// <remarks>
    /// Will raise events when needed. 
    /// </remarks>

    [RequireComponent(typeof(ObjectSelector), typeof(PickupAndDrag))]
    public class InputManager : SingletonMonobehaviour<InputManager>
    {
        [field: SerializeField]
        public InputReader InputReader { get; private set; } = null;

        [field: SerializeField]
        [Tooltip("Define a layerMask to select the object in")]
        public LayerMask DiceLayerMask { get; private set; }


        private PickupAndDrag _pickupAndDrag;
        private ObjectSelector _objectSelector;
        private Selectable[] selectables;

        // Events for the InputManager
        public event Action<Die> OnDieHoverEnter;
        public event Action<Die> OnDieHoverExit;
        public event Action<Die> OnDieTapped; 
        public event Action<Die> OnDieHoldEnter; 
        public event Action<Die> OnDieHoldExit;



        private void Start()
        {
            Assert.IsNotNull(_objectSelector, "Object selector was null");
            Assert.IsNotNull(_pickupAndDrag, "Pick up and drag component was null");
            Assert.IsTrue(_objectSelector.PickupLayerMask == LayerMask.GetMask("Dice"), "The Object Selector's PickupLayerMask should be set to ONLY the Dice layer");
        }

        protected override void Awake()
        {
            base.Awake();


            selectables = FindObjectsOfType<Selectable>(true);

            if (!TryGetComponent<PickupAndDrag>(out _pickupAndDrag))
            {
                Debug.LogError("No Pickup and Drag component found on InputController");
            }
            if(!TryGetComponent<ObjectSelector>(out _objectSelector))
            {
                Debug.LogError("No Object Selector component found on InputController");
            }

        }

        private void OnEnable()
        {
            if (InputReader == null)
                Debug.LogWarning("Input Reader is not assigned in Input Manager");
            else
            {
                InputReader.Tap += Tap;
                InputReader.Hold += Hold;
            }

            InputReader.Input.ScreenActions.Point.performed += Point;
            InputReader.Input.ScreenActions.Navigate.performed += OnNavigate;
        }

        private void OnDisable()
        {
            if (InputReader != null)
            {
                InputReader.Tap -= Tap;
                InputReader.Hold -= Hold;
            }


            InputReader.Input.ScreenActions.Point.performed -= Point;
            InputReader.Input.ScreenActions.Navigate.performed -= OnNavigate;
        }


        private void FixedUpdate()
        {
            if (_pickupAndDrag.SelectedObject == null) //Nothing is being dragged
                return;

            Vector3 origin = _pickupAndDrag.SelectedObject.transform.position;
            
            Vector3 target = Vector3.zero;
            if (!InputReader.Input.ScreenActions.Point.IsInProgress()) // Pointer (mouse/touch)
            {
                Plane plane = new Plane(Vector3.up, origin);
                Ray ray = Camera.main.ScreenPointToRay(InputReader.ScreenPointerLocation);
                if (plane.Raycast(ray, out float distance))
                {
                    target = ray.GetPoint(distance);
                }
            }
            else if (!InputReader.Input.ScreenActions.Navigate.IsInProgress()) // Gamepad stick
            {
                Vector2 direction = InputReader.NavigateAxisDirection;
                origin += new Vector3(direction.x, 0f, direction.y) /* * speed */;
            }

            _pickupAndDrag.HandleDrag(target);
        }



        private void Tap()
        {
            if (TryToGetDie(out Die die))
                OnDieTapped?.Invoke(die);
        }

        private void Hold(bool performed)
        {
            if (performed)
            {
                if (TryToGetDie(out Die die))
                {
                    InputReader.DisableScreenActions();
                    GameObject selectedObject = die.gameObject;
                    _pickupAndDrag.HandlePickup(selectedObject);
                    OnDieHoldEnter?.Invoke(die);
                }
            }
            else
            {
                InputReader.EnableScreenActions();
                GameObject droppedGameObject = _pickupAndDrag.HandleDrop();
                if (TryToGetDie(droppedGameObject, out Die die))
                    OnDieHoldExit?.Invoke(die);
            }
        }

        private bool TryToGetDie(out Die die)
        {
            return TryToGetDie(EventSystem.current.currentSelectedGameObject, out die);
        }
        private bool TryToGetDie(GameObject obj, out Die die)
        {
            die = null;
            if (obj == null)
                return false;

            die = obj.GetComponent<Die>();
            if (die == null)
                return false;

            return true;
        }

        /// <summary>
        /// Invokes when te pointer is moved
        /// </summary>
        /// <param name="context"></param>
        private void Point(InputAction.CallbackContext context)
        {
            Cursor.visible = true;
            if (_pickupAndDrag.SelectedObject != null)
                return;

            Vector2 pointerPosition = context.ReadValue<Vector2>();

            //Edge case where the objects are very close and the raycast might hit a different object
            GameObject currGameObject = EventSystem.current.currentSelectedGameObject;
            GameObject newGameObject = Helpers.GetObjectFromScreen(pointerPosition, DiceLayerMask); //Prevents flickering

            // If the object has changed, handle the change
            if (currGameObject != newGameObject)
            {
                if (currGameObject != null)
                    if (currGameObject.TryGetComponent(out Die die))
                        OnDieHoverExit?.Invoke(die);

                if (newGameObject != null)
                {
                    if (newGameObject.TryGetComponent(out Die die))
                    {
                        EventSystem.current.SetSelectedGameObject(newGameObject);
                        OnDieHoverEnter?.Invoke(die);
                    }
                }
                else EventSystem.current.SetSelectedGameObject(null);
            }
        }

        private void OnNavigate(InputAction.CallbackContext context) //Only Controllers
        {
            Cursor.visible = false;
            if (EventSystem.current.currentSelectedGameObject == null) SelectFirstSelectableObject();
        }

        private void SelectFirstSelectableObject()
        {
            selectables.First(
                (selectable) => { return selectable.interactable && selectable.gameObject.activeInHierarchy; }
                ).Select();
        }

        private void OnDrawGizmos()
        {

            Gizmos.DrawSphere(Vector3.zero, 1f);

        }
    }
}
