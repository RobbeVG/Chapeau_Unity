using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Seacore.Game
{
    interface IInputReader 
    {
        public Vector2 ScreenPointerLocation { get; }
        public Vector2 NavigateAxisDirection { get; }

        public void EnableDiceActions();
        public void DisableDiceActions();
        public void EnableScreenActions();
        public void DisableScreenActions();
    }

    [CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
    public class InputReader : ScriptableObject, IInputReader
    {
        public event Action Tap = delegate { };
        public event Action<bool> Hold = delegate { };

        private bool isHolding = false;

        public ChapeauInputActions Input { get; private set; }

        public Vector2 ScreenPointerLocation => Input.ScreenActions.Point.ReadValue<Vector2>();
        public Vector2 NavigateAxisDirection => Input.ScreenActions.Navigate.ReadValue<Vector2>();

        void OnEnable()
        {
            if (Input == null)
                Input = new ChapeauInputActions();

            Input.Enable();
            Input.DiceActions.Tap.performed += OnTap;
            Input.DiceActions.Hold.performed += OnHoldPerformed;
        }
        void OnDisable()
        {
            Input.Disable();
            Input.DiceActions.Tap.performed -= OnTap;
            Input.DiceActions.Hold.performed -= OnHoldPerformed;
            Input.DiceActions.Hold.canceled -= OnHoldCanceled;
        }

        /// <summary>
        /// The performed method when Tap is performed
        /// </summary>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void OnTap(InputAction.CallbackContext context) => Tap.Invoke();
        /// <summary>
        /// The performed method when Hold is performed.
        /// </summary>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void OnHoldPerformed(InputAction.CallbackContext context)
        {
            Hold.Invoke(isHolding = true);
            Input.DiceActions.Hold.canceled += OnHoldCanceled;
        }
        /// <summary>
        /// The performed method when Hold is canceled once it was performed.
        /// </summary>
        /// <param name="context"><inheritdoc cref="InputAction.CallbackContext"/> </param>
        private void OnHoldCanceled(InputAction.CallbackContext context)
        {
            Hold.Invoke(isHolding = false);
            Input.DiceActions.Hold.canceled -= OnHoldCanceled;
        }

        public void EnableScreenActions() => Input.ScreenActions.Enable();
        public void DisableScreenActions() => Input.ScreenActions.Disable();
        public void EnableDiceActions() => Input.DiceActions.Enable();
        public void DisableDiceActions() => Input.DiceActions.Disable();
    }
}
