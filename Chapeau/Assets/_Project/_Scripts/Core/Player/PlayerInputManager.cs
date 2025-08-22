using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Seacore.Common;
using UnityEngine.Scripting;
using System;

namespace Seacore.Game
{
    /// <summary>
    /// A specified player controller that handles the input from the input System.
    /// </summary>
    /// <remarks>
    /// Will raise events when needed. 
    /// </remarks>

    [RequireComponent(typeof(ObjectSelector), typeof(PickupAndDrag))]
    public class PlayerInputManager : SingletonMonobehaviour<PlayerInputManager>
    {
        private PickupAndDrag _pickupAndDragComponent = null;
        private ObjectSelector _objectSelector = null;

        // Events for the input actions
        private ChapeauInputActions _inputActions;
        private InputAction _tap = null;
        private InputAction _hold = null;
        private InputAction _point = null;

        // Events for the InputManager
        public event Action<Die> OnDieHoverEnter;
        public event Action<Die> OnDieHoverExit;
        public event Action<Die> OnDieTapped; 
        public event Action<Die> OnDieHoldEnter; 
        public event Action<Die> OnDieHoldExit;

        [SerializeField, ReadOnly]
        private bool _pointing = false;
        public bool Pointing
        {
            get => _pointing;
            set
            {
                _pointing = value;
                Debug.Log($"{_pointing} Pointing/Hovering");
                if (_pointing) 
                    _point.Enable();
                else 
                    _point.Disable();
            }
        }

        [SerializeField, ReadOnly]
        private bool _grabbing = false;
        public bool Grabbing
        {
            get => _grabbing;
            set
            {
                _grabbing = value;
                _pickupAndDragComponent.enabled = _grabbing; // Enable or disable the pickup and drag component based on grabbing state
                Debug.Log($"{_grabbing} Grabbing");
                if (_grabbing) 
                    _hold.Enable();
                else
                    _hold.Disable();
            }
        }

        private void Start()
        {
            Assert.IsNotNull(_objectSelector, "Object selector was null");
            Assert.IsNotNull(_pickupAndDragComponent, "Pick up and drag component was null");
            Assert.IsTrue(_objectSelector.PickupLayerMask == LayerMask.GetMask("Dice"), "The Object Selector's PickupLayerMask should be set to ONLY the Dice layer");
        }

        protected override void Awake()
        {
            base.Awake();
            _inputActions = new ChapeauInputActions();

            if(!TryGetComponent<PickupAndDrag>(out _pickupAndDragComponent))
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
            _tap = _inputActions.InGame.Tap;
            _tap.Enable(); //-> Only disable when the appropiate state is active
            _tap.performed += Tap;

            _hold = _inputActions.InGame.Hold;
            if (_grabbing)
                _hold.Enable(); //-> Only enable when the appropiate state is active
            _hold.performed += HoldPerformed;
            _pickupAndDragComponent.enabled = _grabbing;
            //Canceling the hold will only be eneabled when the action is performed.

            _point = _inputActions.InGame.Point;
            if (_pointing)
                _point.Enable(); //-> Only enable when the appropiate state is active
            _point.performed += Point;
        }

        private void OnDisable()
        {
            _tap.Disable();
            _hold.Disable();
            _point.Disable();
        }

        /// <summary>
        /// The performed method when Tap is performed
        /// </summary>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void Tap(InputAction.CallbackContext context)
        {

            GameObject selectedObject = _objectSelector.ObjectOnCursor;
            if (selectedObject == null)
                return;

            Debug.Log($"Tapped {selectedObject}");

            Die die = selectedObject.GetComponent<Die>();
            if (die == null)
                return; 

            OnDieTapped?.Invoke(die);
        }

        /// <summary>
        /// The performed method when Hold is performed.
        /// </summary>
        /// <remarks>
        /// Only now the cancelation will take place
        /// </remarks>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void HoldPerformed(InputAction.CallbackContext context)
        {
            GameObject selectedObject = _objectSelector.ObjectOnCursor;
            if (selectedObject == null)
                return;

            _pickupAndDragComponent.HandlePickup(selectedObject);
            _hold.canceled += HoldCanceled;

            if (selectedObject.TryGetComponent(out Die die))
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
            GameObject droppedGameObject = _pickupAndDragComponent.HandleDrop();
            if (droppedGameObject.TryGetComponent(out Die die))
                OnDieHoldExit?.Invoke(die);
      
            _hold.canceled -= HoldCanceled;
        }

        /// <summary>
        /// Invokes when te pointer is moved
        /// </summary>
        /// <param name="context"></param>
        private void Point(InputAction.CallbackContext context)
        {
            if (_pickupAndDragComponent.SelectedObject != null)
                return;

            Vector2 pointerPosition = context.ReadValue<Vector2>();

            GameObject prevGameObject = _objectSelector.ObjectOnCursor;
            _objectSelector.SetObjectFromPosition(pointerPosition);
            GameObject newGameObject = _objectSelector.ObjectOnCursor;

            // If the object has changed, handle the change
            if (prevGameObject != newGameObject)
            {
                if (prevGameObject != null)
                {
                    if (prevGameObject.TryGetComponent(out Die die))
                        OnDieHoverExit?.Invoke(die);
                }

                if (newGameObject != null)
                {
                    if (newGameObject.TryGetComponent(out Die die))
                        OnDieHoverEnter?.Invoke(die);
                }
            }
        }

        private static ref readonly Vector2 GetInputLocation()
        {
            return ref Pointer.current.position.value;
        }
    }
}
