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
            Reveal, Chapeau, DeclareConfirm, Roll
        }

        [SerializeField]
        RoundStateMachine roundSM = null;

        [SerializeField]
        private GameObject background;

        [SerializeField]
        private DeclareMenu declareMenu = null;

        [SerializeField]
        public UIButtonManager<ButtonTypes> buttonManager = new UIButtonManager<ButtonTypes>();

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
            background.SetActive(false);
            declareMenu.gameObject.SetActive(false);
            buttonManager.HideAllButtons();
        }

        private void OnEnable()
        {
            roundSM.OnStateChanged += OnRoundStateChange;
            declareMenu.OnEditDeclareRoll += SetDeclareConfirmButtonInteractable;
        }

        private void OnDisable()
        {
            roundSM.OnStateChanged -= OnRoundStateChange;
            declareMenu.OnEditDeclareRoll -= SetDeclareConfirmButtonInteractable;
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

                    SetUpDeclareUI();
                    break;
                case RoundStateMachine.RoundState.Received:
                    //Remove previous onClick events if there
                    buttonManager[ButtonTypes.DeclareConfirm].onClick.RemoveListener(roundSM.TransitionToReceived);

                    SetUpReceivedUI();
                    SetDeclareConfirmButtonInteractable();
                    SetUpDeclareUI();
                    break;
                case RoundStateMachine.RoundState.RollSetup:
                    //Remove previous onClick events if there
                    buttonManager[ButtonTypes.Reveal].onClick.RemoveListener(roundSM.TransitionToRollSetup);

                    buttonManager[ButtonTypes.Roll].gameObject.SetActive(true); //Which can be inside or outside
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
            buttonManager[ButtonTypes.Roll].onClick.AddListener(roundSM.TransitionToDeclare);

            buttonManager[ButtonTypes.Roll].gameObject.SetActive(true); //Which can be inside or outside
            buttonManager[ButtonTypes.Chapeau].gameObject.SetActive(true);
        }

        private void SetUpDeclareUI()
        {
            //Set onClick event
            buttonManager[ButtonTypes.DeclareConfirm].onClick.AddListener(roundSM.TransitionToReceived);

            buttonManager[ButtonTypes.Reveal].gameObject.SetActive(true);
            buttonManager[ButtonTypes.DeclareConfirm].gameObject.SetActive(true);
            declareMenu.gameObject.SetActive(true);
        }

        private void SetDeclareConfirmButtonInteractable()
        {
            buttonManager[ButtonTypes.DeclareConfirm].interactable = roundSM.DeclaredRoll > roundSM.CurrentRoll;
        }
    }
}
