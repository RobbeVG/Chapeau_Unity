using Seacore.Common;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Seacore.Game
{
    /// <summary>
    /// A specified player controller that handles the input from the input System.
    /// </summary>
    /// <remarks>
    /// Will raise events when needed. 
    /// </remarks>

    [RequireComponent(typeof(PickupAndDrag))]
    public class InputActionManager : MonoBehaviour, IInputActivator
    {
        [field: SerializeField]
        [Tooltip("Define a layerMask to select the object in")]
        public LayerMask DiceLayerMask { get; private set; }

        private PickupAndDrag _pickupAndDrag;
        private Selectable[] selectables;
        private PhysicsRaycaster _physicsRaycaster;

        // Events for the InputManager
        public event Action<Die> OnDieTapped; 
        public event Action<Die> OnDieHoldEnter; 
        public event Action<Die> OnDieHoldExit;
        public event Action<bool> OnDiceActionsToggleChanged;
        private InputReader _inputReader = null;

        protected void Awake()
        {
            _inputReader = new InputReader();
            selectables = FindObjectsOfType<Selectable>(true);

            if (!TryGetComponent<PickupAndDrag>(out _pickupAndDrag))
            {
                Debug.LogError("No Pickup and Drag component found on InputController");
            }

            if (!Camera.main.TryGetComponent<PhysicsRaycaster>(out _physicsRaycaster))
            {
                Debug.LogError("No Physics Raycaster component found on InputController");
            }
        }
        private void Start()
        {
            if (_inputReader == null)
            {
                Debug.LogError("Input Reader is not assigned in Input Manager");
                return;
            }

            _inputReader.OnTap = Tap;
            _inputReader.OnHold = Hold;
            _inputReader.OnDeHold = Release;
            _inputReader.OnPointerInput = PointerMoved;
            _inputReader.OnNavigateInput = NavigateMoved;

            DisableDiceActions();

            //At start, the Input System UI module assigns it's own InputActionAsset to the EventSystem
            //This ensures our created inputReader asset (ChapeauInputActions) gets set in the UI Input module
            EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset = _inputReader.Asset;
        }

        private void FixedUpdate()
        {
            if (_pickupAndDrag.SelectedObject == null) //Nothing is being dragged
                return;

            DoDrag();
        }

        private void Tap()
        {
            Debug.Log("Tap detected");

            if (TryToGetDie(out Die die))
                OnDieTapped?.Invoke(die);
        }
        private void Hold()
        {
            Debug.Log("Hold detected");

            if (TryToGetDie(out Die die))
            {
                _inputReader.DisableScreenActions();
                _physicsRaycaster.enabled = false;


                GameObject selectedObject = die.gameObject;
                _pickupAndDrag.HandlePickup(selectedObject);
                OnDieHoldEnter?.Invoke(die);
            }
        }
        private void Release()
        {
            _inputReader.EnableScreenActions();
            _physicsRaycaster.enabled = true;

            GameObject droppedGameObject = _pickupAndDrag.HandleDrop();
            if (TryToGetDie(droppedGameObject, out Die die))
                OnDieHoldExit?.Invoke(die);

        }
        private void DoDrag()
        {
            Vector3 origin = _pickupAndDrag.SelectedObject.transform.position;

            Vector3 target = Vector3.zero;
            if (_inputReader.IsPointerBeingUsed) // Pointer (mouse/touch)
            {
                Plane plane = new Plane(Vector3.up, origin);
                Ray ray = Camera.main.ScreenPointToRay(_inputReader.DiceScreenPointerLocation);
                if (plane.Raycast(ray, out float distance))
                {
                    target = ray.GetPoint(distance);
                }
            }
            else if (_inputReader.IsNavigatorBeingUsed) // Gamepad stick
            {
                Vector2 direction = _inputReader.DiceNavigateAxisDirection;
                origin += new Vector3(direction.x, 0f, direction.y) /* * speed */;
            }

            _pickupAndDrag.HandleDrag(target);
        }
        private void PointerMoved()
        {
            Cursor.visible = true;
        }
        private void NavigateMoved() //Only Controllers
        {
            Cursor.visible = false;
            if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
            {
                // Select the first selectable object.
                selectables.First(
                    (selectable) => { return selectable.interactable && selectable.gameObject.activeInHierarchy; }
                ).Select();
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

        public void EnableDiceActions()
        {
            _physicsRaycaster.enabled = true; // pointer events
            _inputReader.EnableDiceActions();
            OnDiceActionsToggleChanged?.Invoke(true);
        }

        public void DisableDiceActions()
        {
            _physicsRaycaster.enabled = false;
            _inputReader.DisableDiceActions();
            OnDiceActionsToggleChanged?.Invoke(false);
        }

        public void EnableScreenActions() => _inputReader.EnableScreenActions();

        public void DisableScreenActions() => _inputReader.DisableScreenActions();
    }
}
