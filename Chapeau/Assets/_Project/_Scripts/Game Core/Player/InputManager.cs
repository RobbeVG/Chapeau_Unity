using Seacore.Common;
using System;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        private PickupAndDrag _pickupAndDrag;
        private ObjectSelector _objectSelector;
        
        private Selectable[] selectables;

        private Vector3 _dragOffset;
        private bool _isDraggingWithController;
        private Vector3 _controllerDragPosition;

        // Events for the input actions - Not allowed to be assigned in constructor of this class 
        private ChapeauInputActions _inputActions = null;

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

            //TOOO - remove this
            SetDiceActions(true);
        }

        protected override void Awake()
        {
            base.Awake();
            _inputActions = new ChapeauInputActions();


            _inputActions.ScreenActions.Enable();

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
            _inputActions.DiceActions.Tap.performed += Tap;
            _inputActions.DiceActions.Hold.performed += Hold;
            _inputActions.ScreenActions.Point.performed += Point;
            _inputActions.ScreenActions.Navigate.performed += OnNavigate;
        }

        private void OnDisable()
        {
            _inputActions.DiceActions.Tap.performed -= Tap;
            _inputActions.DiceActions.Hold.performed -= Hold;
            _inputActions.ScreenActions.Point.performed -= Point;
            _inputActions.ScreenActions.Navigate.performed -= OnNavigate;

        }

        private void FixedUpdate()
        {
            if (_pickupAndDrag.SelectedObject == null)
                return;

            if (_isDraggingWithController)
                _pickupAndDrag.HandleDrag(_controllerDragPosition);
            else
                _pickupAndDrag.HandleDrag(GetMouseWorldPosition() + _dragOffset);
        }

        public void SetDiceActions(bool toggle)
        {
            if (toggle)
            {
                _inputActions.DiceActions.Enable();
                _pickupAndDrag.enabled = true;
            }
            else
            {
                _inputActions.DiceActions.Disable();
                _pickupAndDrag.enabled = false;
            }
        }

        /// <summary>
        /// The performed method when Tap is performed
        /// </summary>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void Tap(InputAction.CallbackContext context)
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject == null)
                return;

            Die die = selectedObject.GetComponent<Die>();
            if (die == null)
                return;

            Debug.Log($"Tapped {selectedObject}");
            OnDieTapped?.Invoke(die);
        }

        /// <summary>
        /// The performed method when Hold is performed.
        /// </summary>
        /// <remarks>
        /// Only now the cancelation will take place
        /// </remarks>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void Hold(InputAction.CallbackContext context)
        {
            InputDevice device = context.control.device;

            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
            if (selectedObject == null)
                return;
            Die die = selectedObject.GetComponent<Die>();

            if (die == null)
                return;

            _inputActions.ScreenActions.Disable();
            _inputActions.DiceActions.Hold.canceled += HoldCanceled;

            _pickupAndDrag.HandlePickup(selectedObject);

            Debug.Log($"Holding {selectedObject}");

            // Detect which device triggered the action
            Assert.IsNotNull(device, "The device that triggered the hold action was null");

            if (ReferenceEquals(device, Gamepad.current))
            {
                _isDraggingWithController = true;
                _controllerDragPosition = selectedObject.transform.position;
                _dragOffset = Vector3.zero;
            }
            else
            {
                _isDraggingWithController = false;
                _dragOffset = selectedObject.transform.position - GetMouseWorldPosition();
            }
            
            OnDieHoldEnter?.Invoke(die);
        }

        /// <summary>
        /// The performed method when Hold is canceled
        /// </summary>
        /// <remarks>
        /// Unsubscribes the Cancelation method of hold.
        /// </remarks>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void HoldCanceled(InputAction.CallbackContext context)
        {
            _inputActions.DiceActions.Hold.canceled -= HoldCanceled; //Ohterwise it already gets ivoked when the action is not yet performed.
            _inputActions.ScreenActions.Enable();

            GameObject droppedGameObject = _pickupAndDrag.HandleDrop();
            if (droppedGameObject == null) return; // When nothing was picked up

            if (droppedGameObject.TryGetComponent(out Die die))
                OnDieHoldExit?.Invoke(die);

            Debug.Log($"Dropped {droppedGameObject}");

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
            GameObject prevGameObject = _objectSelector.ObjectOnCursor;
            _objectSelector.SetObjectFromPosition(pointerPosition);
            GameObject newGameObject = _objectSelector.ObjectOnCursor;

            // If the object has changed, handle the change
            if (prevGameObject != newGameObject)
            {
                if (prevGameObject != null)
                {
                    if (prevGameObject.TryGetComponent(out Die die))
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        OnDieHoverExit?.Invoke(die);
                    }
                }

                if (newGameObject != null)
                {
                    if (newGameObject.TryGetComponent(out Die die))
                    {
                        EventSystem.current.SetSelectedGameObject(newGameObject);
                        OnDieHoverEnter?.Invoke(die);
                    }
                }
            }
        }

        private void OnNavigate(InputAction.CallbackContext context) //Only Controllers
        {
            Cursor.visible = false;

            InputDevice device = context.control.device;

            if (_pickupAndDrag.SelectedObject != null && _isDraggingWithController)
            {
                Vector2 direction = context.ReadValue<Vector2>();
                float moveSpeed = 5f; // Tune as needed
                _controllerDragPosition += new Vector3(direction.x, 0, direction.y) * moveSpeed * Time.deltaTime;
            }
            else if (EventSystem.current.currentSelectedGameObject == null)
            {
                SelectFirstSelectableObject();
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane ground = new Plane(Vector3.up, new Vector3(0, _pickupAndDrag.PickupHeightOffset, 0));
            if (ground.Raycast(ray, out float enter))
                return ray.GetPoint(enter);
            return Vector3.zero;
        }

        private void SelectFirstSelectableObject()
        {


            foreach (Selectable selectable in selectables)
            {
                if (selectable.interactable && selectable.gameObject.activeInHierarchy)
                {
                    EventSystem.current.SetSelectedGameObject(selectable.gameObject);
                    return;
                }
            }
        }
    }
}
