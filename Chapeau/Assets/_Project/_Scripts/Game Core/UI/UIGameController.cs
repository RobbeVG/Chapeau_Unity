using System;
using System.Linq;
using UnityEngine;
using Seacore.Game.UI;
using Seacore.Common;
using Seacore.Common.Statemachine;
using Reflex.Attributes;
using Seacore.UI;


namespace Seacore.Game
{
    [RequireComponent(typeof(UIButtonManager<ButtonTypes>))]
    public class UIGameController : MonoBehaviour
    {
        [Serializable]
        public enum ButtonTypes
        {
            Reveal,
            Chapeau,
            DeclareConfirm, 
            Roll
        }

        [Inject]
        private InputActionManager _inputManager = null;
        [Inject]
        private GameRoundManager roundManager = null;

        [SerializeField]
        private DeclareMenu _declareMenu = null;

        [SerializeField]
        public UIButtonManager<ButtonTypes> buttonManager = new UIButtonManager<ButtonTypes>();

        [SerializeField]
        private DiceController _diceController = null;

        [SerializeField]
        private DiceManager _diceManager = null;

        [SerializeField]
        private RollDisplay _rollDisplay = null;

        private void Start()
        {
            Debug.Log("Start");

            DisableAll();
            OnRoundStateEnter(roundManager.CurrentState);
            SetDeclareConfirmButtonInteractable();
        }

        private void DisableAll()
        {
            _declareMenu.gameObject.SetActive(false);
            buttonManager.HideAllButtons();
        }

        private void OnEnable()
        {
            IStateMachineEvents<RoundState> stateEvents = roundManager.StateMachineEvents;
            stateEvents.OnStateEnter += OnRoundStateEnter;
            stateEvents.OnStateExit += OnRoundStateExit;
            
            _inputManager.OnDieTapped += SetRollButtonInteractable;
            _inputManager.OnDieHoldExit += SetRollButtonInteractable;
            

            buttonManager[ButtonTypes.Reveal].onClick.AddListener(OnRevealButtonClick);
            buttonManager[ButtonTypes.DeclareConfirm].onClick.AddListener(ToStateReceived); 
            buttonManager[ButtonTypes.Roll].onClick.AddListener(ToStateDeclare);

            _declareMenu.OnEditDeclareRoll += SetDeclareConfirmButtonInteractable;
        }

        private void OnDisable()
        {
            IStateMachineEvents<RoundState> stateEvents = roundManager.StateMachineEvents;
            stateEvents.OnStateEnter -= OnRoundStateEnter;
            stateEvents.OnStateExit -= OnRoundStateExit;
            
            _inputManager.OnDieTapped -= SetRollButtonInteractable;
            _inputManager.OnDieHoldExit -= SetRollButtonInteractable;
            


            buttonManager[ButtonTypes.Reveal].onClick.RemoveListener(OnRevealButtonClick);

            buttonManager[ButtonTypes.DeclareConfirm].onClick.RemoveListener(ToStateReceived);
            buttonManager[ButtonTypes.Roll].onClick.RemoveListener(ToStateDeclare);
            _declareMenu.OnEditDeclareRoll -= SetDeclareConfirmButtonInteractable;
        }

        private void OnRoundStateExit(RoundState stateType)
        {
            switch (stateType)
            {
                case RoundState.Received:
                    buttonManager[ButtonTypes.Reveal].onClick.RemoveListener(ToStateRollSetup); //Exception has to be added
                    break;
                default:
                    break;
            }
        }

        private void OnRoundStateEnter(RoundState stateType)
        {
            //Declare visble
            buttonManager[ButtonTypes.DeclareConfirm].gameObject.SetActive(true);
            _declareMenu.gameObject.SetActive(true);

            SetDeclareConfirmButtonInteractable(); //You need to readjust the button because of new state!
            switch (stateType)
            {
                case RoundState.Declare:
                    buttonManager[ButtonTypes.Reveal].gameObject.SetActive(true);

                    //If comming from receive (Pressed Roll)
                    buttonManager[ButtonTypes.Chapeau].gameObject.SetActive(false);
                    buttonManager[ButtonTypes.Roll].gameObject.SetActive(false);
                    break;
                case RoundState.Received:
                    buttonManager[ButtonTypes.Reveal].onClick.AddListener(ToStateRollSetup);

                    buttonManager[ButtonTypes.Reveal].gameObject.SetActive(true);
                    buttonManager[ButtonTypes.Chapeau].gameObject.SetActive(true);
                    buttonManager[ButtonTypes.Roll].gameObject.SetActive(true);
                    break;
                case RoundState.RollSetup:
                    buttonManager[ButtonTypes.Reveal].gameObject.SetActive(false);
                    buttonManager[ButtonTypes.Chapeau].gameObject.SetActive(false);
                    break;
                case RoundState.Chapeau:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// On click callback for the reveal button!
        /// </summary>
        /// <remarks>
        /// Needs implementation because it does not transition to another state.
        /// </remarks>
        private void OnRevealButtonClick()
        {
            _diceController.RevealDice();
            buttonManager[ButtonTypes.Reveal].gameObject.SetActive(false);
            if (roundManager.CurrentState.HasFlag(RoundState.Received))
            {
                buttonManager[ButtonTypes.Roll].gameObject.SetActive(true); 
                buttonManager[ButtonTypes.Roll].interactable = false; 
            }
        }

        private void ToStateRollSetup()
        {
            roundManager.StateMachineTransitions.TransitionToState(RoundState.RollSetup);
        }
        private void ToStateReceived()
        {
            roundManager.StateMachineTransitions.TransitionToState(RoundState.Received);
            buttonManager[ButtonTypes.Roll].gameObject.SetActive(false); 
        }
        private void ToStateDeclare()
        {
            roundManager.StateMachineTransitions.TransitionToState(RoundState.Declare);
        }

        private void SetRollButtonInteractable(Die _)
        {
            buttonManager[ButtonTypes.Roll].interactable = _diceManager.DiceContainers.Values.Any(info => info.State.HasFlag(DieState.ToRoll));
        }
        private void SetDeclareConfirmButtonInteractable()
        {
            buttonManager[ButtonTypes.DeclareConfirm].interactable = roundManager.RoundRolls.DeclaredRoll > roundManager.RoundRolls.CurrentRoll;
        }
    }
}
