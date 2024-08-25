using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Seacore
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
        private RoundStateMachine roundSM = null;

        [SerializeField]
        private GameObject _background = null;

        [SerializeField]
        private DeclareMenu _declareMenu = null;

        [SerializeField]
        public UIButtonManager<ButtonTypes> buttonManager = new UIButtonManager<ButtonTypes>();

        [SerializeField]
        private CircleController _circleController = null;

        [SerializeField]
        private DiceController _diceController = null;

        private void Awake()
        {
            Assert.IsNotNull(roundSM, "Round State Machine Controller in the UIManager cannot be null");
        }

        private void Start()
        {
            //Clear all menu's?
            OnRoundStateChange();
            SetDeclareConfirmButtonInteractable();
        }

        private void DisableAll()
        {
            _background.SetActive(false);
            _declareMenu.gameObject.SetActive(false);
            buttonManager.HideAllButtons();
        }

        private void OnEnable()
        {
            roundSM.OnStateChanged += OnRoundStateChange;
            _declareMenu.OnEditDeclareRoll += SetDeclareConfirmButtonInteractable;
        }

        private void OnDisable()
        {
            roundSM.OnStateChanged -= OnRoundStateChange;
            _declareMenu.OnEditDeclareRoll -= SetDeclareConfirmButtonInteractable;
        }

        private void OnRoundStateChange()
        {
            DisableAll();
            RoundStateMachine.RoundState stateType = roundSM.CurrentStateKey;

            switch (stateType)
            {
                case RoundStateMachine.RoundState.Declare:
                    //Remove previous onClick events if there
                    buttonManager[ButtonTypes.Roll].onClick.RemoveListener(roundSM.TransitionToDeclare);
                    buttonManager[ButtonTypes.Reveal].onClick.RemoveListener(roundSM.TransitionToRollSetup);
                    buttonManager[ButtonTypes.Reveal].onClick.RemoveListener(_diceController.RevealDice);

                    buttonManager[ButtonTypes.Reveal].gameObject.SetActive(true);

                    SetUpDeclareUI();
                    break;
                case RoundStateMachine.RoundState.Received:
                    //Remove previous onClick events if there
                    buttonManager[ButtonTypes.DeclareConfirm].onClick.RemoveListener(roundSM.TransitionToReceived);
                    buttonManager[ButtonTypes.Reveal].gameObject.SetActive(true);

                    SetUpReceivedUI();
                    SetDeclareConfirmButtonInteractable();
                    SetUpDeclareUI();
                    break;
                case RoundStateMachine.RoundState.RollSetup:
                    //Remove previous onClick events if there
                    buttonManager[ButtonTypes.Reveal].onClick.RemoveListener(roundSM.TransitionToRollSetup);

                    buttonManager[ButtonTypes.Roll].gameObject.SetActive(true); //Which can be inside or outside
                    SetUpDeclareUI();
                    //Show view choice menu
                    //Unshow dice
                    break;
                case RoundStateMachine.RoundState.Chapeau:
                    //Show end game
                    break;
                default:
                    break;
            }

            //background.SetActive(stateType == RoundStateMachine.RoundState.Roll || stateType == RoundStateMachine.RoundState.PassOn ||
            //    (stateType == RoundStateMachine.RoundState.Declare && (roundSM.PreviousRoundState.Type == RoundStateMachine.RoundState.Roll || roundSM.PreviousRound.Type == RoundStateMachine.RoundState.PassOn)));
            //chooseActionMenu.SetActive(stateType == RoundStateMachine.RoundState.Roll || stateType == RoundStateMachine.RoundState.PassOn);
            //declareMenu.SetActive(stateType == RoundStateMachine.RoundState.Declare);
        }

        private void SetUpReceivedUI()
        {
            //Set onClick event
            buttonManager[ButtonTypes.Reveal].onClick.AddListener(roundSM.TransitionToRollSetup);
            buttonManager[ButtonTypes.Reveal].onClick.AddListener(_diceController.RevealDice);
            buttonManager[ButtonTypes.Roll].onClick.AddListener(roundSM.TransitionToDeclare);

            buttonManager[ButtonTypes.Roll].gameObject.SetActive(true); //Which can be inside or outside
            buttonManager[ButtonTypes.Chapeau].gameObject.SetActive(true);
        }

        private void SetUpDeclareUI()
        {
            //Set onClick event
            buttonManager[ButtonTypes.Reveal].onClick.AddListener(_diceController.RevealDice);
            buttonManager[ButtonTypes.DeclareConfirm].onClick.AddListener(roundSM.TransitionToReceived);

            buttonManager[ButtonTypes.DeclareConfirm].gameObject.SetActive(true);
            _declareMenu.gameObject.SetActive(true);
        }

        private void SetDeclareConfirmButtonInteractable()
        {
            buttonManager[ButtonTypes.DeclareConfirm].interactable = roundSM.DeclaredRoll > roundSM.CurrentRoll;
        }

        private bool IsMousePositionInsideChapeau()
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return _circleController.IsPositionInCircle(position);
        }
    }
}
