using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        RoundStateMachineController roundSM = null;

        [SerializeField]
        private GameObject background;
        [SerializeField]
        private GameObject chooseActionMenu;
        [SerializeField]
        private GameObject declareMenu;
        
        private void Awake()
        {
            Assert.IsNotNull(roundSM, "Round State Machine Controller in the Action Menu Controller cannot be 0");
            OnRoundStateChange(roundSM.CurrentRoundState.Type);
        }

        private void OnEnable()
        {
            roundSM.OnRoundStateChange += OnRoundStateChange;
        }

        private void OnDisable()
        {
            roundSM.OnRoundStateChange -= OnRoundStateChange;
        }

        private void OnRoundStateChange(RoundStateType stateType)
        {
            background.SetActive(stateType == RoundStateType.Roll || stateType == RoundStateType.PassOn ||
                (stateType == RoundStateType.Declare && (roundSM.PreviousRoundState.Type == RoundStateType.Roll || roundSM.PreviousRoundState.Type == RoundStateType.PassOn)));
            chooseActionMenu.SetActive(stateType == RoundStateType.Roll || stateType == RoundStateType.PassOn || stateType == RoundStateType.Look);
            declareMenu.SetActive(stateType == RoundStateType.Declare);
        }
    }
}
