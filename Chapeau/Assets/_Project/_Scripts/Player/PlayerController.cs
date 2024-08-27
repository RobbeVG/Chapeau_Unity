using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Seacore
{
    /// <summary>
    /// A specified player controller that handles the input from the input System.
    /// </summary>
    /// <remarks>
    /// Will raise events when needed. 
    /// </remarks>
    public class PlayerInputController : MonoBehaviour
    {
        #region InspectorFields
        [SerializeField]
        [Tooltip("The related Pickup and Drag Component")]
        private PickupAndDrag _pickupAndDragComponent = null;
        [SerializeField]
        [Tooltip("The related diceController Component")]
        private DiceController _diceController = null;
        [SerializeField]
        [Tooltip("The related Object Selector Component")]
        private ObjectSelector _objectSelector = null;
        #endregion

        private ChapeauInputActions _inputActions = null;

        private InputAction _tap = null;
        private InputAction _hold = null;
        private InputAction _location = null;

        private float GetInputPosition { get; set; }

        private void Awake()
        {
            _inputActions = new ChapeauInputActions();
        }

        private void Start()
        {
            Assert.IsNotNull(_objectSelector, "Object selector was null");
            Assert.IsNotNull(_diceController, "Dice Controller was null");
            Assert.IsNotNull(_pickupAndDragComponent, "Pick up and drag component was null");
        }

        private void OnEnable()
        {
            _tap = _inputActions.InGame.Tap;
            _tap.Enable();
            _tap.performed += Tap;

            _hold = _inputActions.InGame.Hold;
            _hold.Enable();
            _hold.performed += HoldPerformed;
            //Canceling the hold will only be eneabled when the action is performed.

            _location = _inputActions.InGame.Location;
            _location.Enable();
        }

        private void OnDisable()
        {
            _tap.Disable();
            _hold.Disable();
            _location.Disable();
        }

        /// <summary>
        /// The performed method when Tap is performed
        /// </summary>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void Tap(InputAction.CallbackContext context)
        {
            GameObject selectedObject = _objectSelector.SelectObject();
            if (selectedObject == null)
                return;

            Die die = selectedObject.GetComponent<Die>();
            if (die == null)
                return;

            _diceController.ToggleDieForRoll(die);
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
            GameObject selectedObject = _objectSelector.SelectObject();
            if (selectedObject == null)
                return;
            
            _pickupAndDragComponent.HandlePickup(selectedObject);
            _hold.canceled += HoldCanceled;
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
            _pickupAndDragComponent.HandleDrop();
            _hold.canceled -= HoldCanceled;
        }

        //private ref readonly Vector2 GetInputLocation()
        //{
        //    return ref Pointer.current.position.value;
        //}
    }
}
