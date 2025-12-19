using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

using Seacore.Game.UI;
using Seacore.Common;

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

        [SerializeField]
        private RoundManager roundManager = null;

        [SerializeField]
        private DeclareMenu _declareMenu = null;

        [SerializeField]
        public UIButtonManager<ButtonTypes> buttonManager = new UIButtonManager<ButtonTypes>();

        [SerializeField]
        private DiceController _diceController = null;

        [SerializeField]
        private DiceManager _diceManager = null;

        private RoundStateMachine _roundSM;
        private RoundContext _roundContext = null;

        private void Awake()
        {
            _roundSM = roundManager.RoundStateMachine;
            _roundContext = roundManager.Context;
            Assert.IsNotNull(_roundSM, "Round State Machine Controller in the UIManager cannot be null");
        }

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
            _roundSM.OnStateEnter += OnRoundStateEnter;
            _roundSM.OnStateExit += OnRoundStateExit;
            
            StartCoroutine(SetInputEvents());
            IEnumerator SetInputEvents()
            {
                yield return new WaitUntil(() => InputManager.Instance != null);
                InputManager.Instance.OnDieTapped += SetRollButtonInteractable;
                InputManager.Instance.OnDieHoldExit += SetRollButtonInteractable;
            }

            buttonManager[ButtonTypes.Reveal].onClick.AddListener(OnRevealButtonClick);
            buttonManager[ButtonTypes.DeclareConfirm].onClick.AddListener(ToStateReceived); 
            buttonManager[ButtonTypes.Roll].onClick.AddListener(ToStateDeclare);

            _declareMenu.OnEditDeclareRoll += SetDeclareConfirmButtonInteractable;
        }

        private void OnDisable()
        {
            _roundSM.OnStateEnter -= OnRoundStateEnter;
            _roundSM.OnStateExit -= OnRoundStateExit;
            InputManager IM = InputManager.Instance;
            if (IM)
            {
                IM.OnDieTapped -= SetRollButtonInteractable;
                IM.OnDieHoldExit -= SetRollButtonInteractable;
            }


            buttonManager[ButtonTypes.Reveal].onClick.RemoveListener(OnRevealButtonClick);

            buttonManager[ButtonTypes.DeclareConfirm].onClick.RemoveListener(ToStateReceived);
            buttonManager[ButtonTypes.Roll].onClick.RemoveListener(ToStateDeclare);
            _declareMenu.OnEditDeclareRoll -= SetDeclareConfirmButtonInteractable;
        }

        private void OnRoundStateExit(RoundStateMachine.RoundState stateType)
        {
            switch (stateType)
            {
                case RoundStateMachine.RoundState.Received:
                    buttonManager[ButtonTypes.Reveal].onClick.RemoveListener(ToStateRollSetup); //Exception has to be added
                    break;
                default:
                    break;
            }
        }

        private void OnRoundStateEnter(RoundStateMachine.RoundState stateType)
        {
            //Declare visble
            buttonManager[ButtonTypes.DeclareConfirm].gameObject.SetActive(true);
            _declareMenu.gameObject.SetActive(true);

            SetDeclareConfirmButtonInteractable(); //You need to readjust the button because of new state!
            switch (stateType)
            {
                case RoundStateMachine.RoundState.Declare:
                    buttonManager[ButtonTypes.Reveal].gameObject.SetActive(true);

                    //If comming from receive (Pressed Roll)
                    buttonManager[ButtonTypes.Chapeau].gameObject.SetActive(false);
                    buttonManager[ButtonTypes.Roll].gameObject.SetActive(false);
                    break;
                case RoundStateMachine.RoundState.Received:
                    buttonManager[ButtonTypes.Reveal].onClick.AddListener(ToStateRollSetup);

                    buttonManager[ButtonTypes.Reveal].gameObject.SetActive(true);
                    buttonManager[ButtonTypes.Chapeau].gameObject.SetActive(true);
                    buttonManager[ButtonTypes.Roll].gameObject.SetActive(true);
                    break;
                case RoundStateMachine.RoundState.RollSetup:
                    buttonManager[ButtonTypes.Reveal].gameObject.SetActive(false);
                    buttonManager[ButtonTypes.Chapeau].gameObject.SetActive(false);
                    break;
                case RoundStateMachine.RoundState.Chapeau:
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
            if (roundManager.CurrentState.HasFlag(RoundStateMachine.RoundState.Received))
            {
                buttonManager[ButtonTypes.Roll].gameObject.SetActive(true); 
                buttonManager[ButtonTypes.Roll].interactable = false; 
            }
        }

        private void ToStateRollSetup()
        {
            _roundSM.TransitionToState(RoundStateMachine.RoundState.RollSetup);
        }
        private void ToStateReceived()
        {
            _roundSM.TransitionToState(RoundStateMachine.RoundState.Received);
            buttonManager[ButtonTypes.Roll].gameObject.SetActive(false); 
        }
        private void ToStateDeclare()
        {
            _roundSM.TransitionToState(RoundStateMachine.RoundState.Declare);
        }

        private void SetRollButtonInteractable(Die _)
        {
            buttonManager[ButtonTypes.Roll].interactable = _diceManager.DiceContainers.Values.Any(info => info.State.HasFlag(DieState.ToRoll));
        }
        private void SetDeclareConfirmButtonInteractable()
        {
            buttonManager[ButtonTypes.DeclareConfirm].interactable = _roundContext.DeclaredRoll > _roundContext.CurrentRoll;
        }



    }
}
