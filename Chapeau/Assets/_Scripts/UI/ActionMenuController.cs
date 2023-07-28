using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore
{
    public class ActionMenuController : MonoBehaviour
    {
        [SerializeField]
        RoundStateMachineController roundSM = null;

        [SerializeField]
        GameObject lookButton;
        [SerializeField]
        GameObject declareButton;
        [SerializeField]
        GameObject chapeauButton;
        [SerializeField]
        GameObject rollButton;
        [SerializeField]
        GameObject inoutButton;

        private void Awake()
        {
            Assert.IsNotNull(roundSM, "Round State Machine Controller in the Action Menu Controller cannot be 0");
        }

        private void OnEnable()
        {
            UpdateActiveButtons(roundSM.CurrentRoundState.Type);
            if (roundSM.CurrentRoundState.Type == RoundStateType.PassOn) // Dirty fix to listen twice. Otherwise the passon action menu to look buttons woeld not work
                roundSM.OnRoundStateChange += UpdateActiveButtons;
        }

        private void OnDisable()
        {
            roundSM.OnRoundStateChange -= UpdateActiveButtons;
        }

        public void UpdateActiveButtons(RoundStateType stateType)
        {
            lookButton.SetActive(stateType == RoundStateType.Roll || stateType == RoundStateType.PassOn);
            inoutButton.SetActive(stateType == RoundStateType.Look);
            declareButton.SetActive(stateType == RoundStateType.Roll || stateType == RoundStateType.PassOn || stateType == RoundStateType.Look);
            chapeauButton.SetActive(stateType == RoundStateType.PassOn);
            rollButton.SetActive(stateType == RoundStateType.PassOn);
        }

        public void OnRoundStateChange(RoundStateType stateType)
        {
            Debug.Log("Round State change in Action Menu - " + stateType.ToString()) ;
        }

        public void OnLookButtonClick()
        {
            roundSM.ChangeRoundState(RoundStateType.Look);
        }

        public void OnDeclareButtonClick()
        {
            roundSM.ChangeRoundState(RoundStateType.Declare);
        }

        public void OnRollButtonClick()
        {
            roundSM.ChangeRoundState(RoundStateType.Roll);
        }

        public void OnChapeauButtonClick()
        {
            roundSM.ChangeRoundState(RoundStateType.Chapeau);
        }

        public void OnInOutButtonClick()
        {
            roundSM.ChangeRoundState(RoundStateType.InOut);
        }
    }
}
