using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seacore.Game
{
    using static ChapeauInputActions;
    interface IInputActivator
    {
        public void EnableDiceActions();
        public void DisableDiceActions();
        public void EnableScreenActions();
        public void DisableScreenActions();
    }

    public class InputReader : IInputActivator
    {
        public Action OnTap = delegate { };
        public Action OnHold = delegate { };
        public Action OnDeHold = delegate { };
        public Action OnNavigateInput = delegate { };
        public Action OnPointerInput = delegate { };

        public ChapeauInputActions Input { get; private set; }
        public InputActionAsset Asset => Input.asset;

        public Vector2 DiceScreenPointerLocation => Input.DiceActions.MovingPointer.ReadValue<Vector2>();
        public Vector2 DiceNavigateAxisDirection => Input.DiceActions.Move.ReadValue<Vector2>();
        public bool IsPointerBeingUsed => Input.DiceActions.MovingPointer.IsInProgress();
        public bool IsNavigatorBeingUsed => Input.DiceActions.Move.IsInProgress();

        public InputReader()
        {
            Input = new ChapeauInputActions();

            Input.Enable();
            Input.DiceActions.Tap.performed += TapPerformed;
            Input.DiceActions.Hold.performed += HoldPerformed;
            Input.DiceActions.Move.performed += MovePerformed;
            Input.UI.Navigate.performed += MovePerformed;
            Input.DiceActions.MovingPointer.performed += PointPerformed;
            Input.UI.Point.performed += PointPerformed;
        }

        ~InputReader()
        {
            Input.Dispose();
        }

        /// <summary>
        /// The performed method when Tap is performed
        /// </summary>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void TapPerformed(InputAction.CallbackContext context) => OnTap.Invoke();
        /// <summary>
        /// The performed method when Hold is performed.
        /// </summary>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void HoldPerformed(InputAction.CallbackContext context)
        {
            OnHold.Invoke();
            Input.DiceActions.Hold.canceled += HoldCanceled;
        }
        /// <summary>
        /// The performed method when Hold is canceled once it was performed.
        /// </summary>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void HoldCanceled(InputAction.CallbackContext context)
        {
            OnDeHold.Invoke();
            Input.DiceActions.Hold.canceled -= HoldCanceled;
        }
        /// <summary>
        /// Handles the performed event for the move input action.
        /// </summary>
        /// <param name="context">The callback context containing information about the performed move input, including the current input
        /// value.</param>
        private void MovePerformed(InputAction.CallbackContext context) => OnNavigateInput.Invoke();
        /// <summary>
        /// Handles the performed event for a pointing input action.
        /// </summary>
        /// <param name="context">The callback context containing information about the input action event. The method reads the current <see
        /// cref="Vector2"/> value from this context and passes it to the <see cref="OnPointerInput"/> event.</param>
        private void PointPerformed(InputAction.CallbackContext context) => OnPointerInput.Invoke();


        public void Enable() => Input.Enable();
        public void Disable() => Input.Disable();
        public void EnableScreenActions()
        {
            Debug.Log("Enabling screen actions");
            Input.UI.Enable();
        }

        public void DisableScreenActions()
        {
            Debug.Log("Disabling screen actions");
            Input.UI.Disable();
        }

        public void EnableDiceActions()
        {
            Debug.Log("Enabling dice actions");
            Input.DiceActions.Enable();
        }

        public void DisableDiceActions()
        {
            Debug.Log("Disabling dice actions");
            Input.DiceActions.Disable();
        }
    }
}
