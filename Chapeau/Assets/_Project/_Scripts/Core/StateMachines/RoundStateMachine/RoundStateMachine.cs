using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    /// <summary>
    /// A child statemachine that specifically is used to progress a round
    /// </summary>
    public class RoundStateMachine : StateMachine<RoundStateMachine.RoundState, RoundStateMachine>
    {
        [Serializable]
        public enum RoundState
        {
            RollSetup, Declare, Received, Chapeau
        }

        [SerializeField]
        private DiceRoller _diceRoller = null;
        [SerializeField]
        private DiceController _diceController = null;
        [SerializeField]
        private UIGameController _uiGameController = null;
        [SerializeField]
        private PickupDragController _pickupDragController = null;

        [Header("Rolls")]
        [SerializeField]
        private Roll physicalRoll;
        [SerializeField]
        private Roll currentRoll;
        [SerializeField]
        private Roll declaredRoll;

        public Roll PhysicalRoll { get { return physicalRoll; } }
        public Roll CurrentRoll { get { return currentRoll; } }
        public Roll DeclaredRoll { get { return declaredRoll; } }
        public DiceRoller DiceRoller { get { return _diceRoller; } }
        public PickupDragController PickUpDragController { get { return _pickupDragController; } }
        public DiceController DiceController { get { return _diceController; } }

        public RoundStateMachine() 
            : base(new Dictionary<RoundState, BaseState<RoundState, RoundStateMachine>>() 
            {
                { RoundState.RollSetup,  new RollSetupState() },
                { RoundState.Declare,  new RollDeclareState() },
                { RoundState.Received,  new ReceivedState() },
                { RoundState.Chapeau,  new ChapeauState() },
            }, currentStateKey: RoundState.Declare) {}

        private void Awake()
        {
            ResetRound();
            _pickupDragController.enabled = false;
        }

        /// <summary>
        /// Reset all Round parameters
        /// </summary>
        public void ResetRound()
        {
            physicalRoll.Clear();
            currentRoll.Clear();
            declaredRoll.Clear();
        }

        public void AddListenerToUIButton(UIGameController.ButtonTypes type, UnityEngine.Events.UnityAction call)
        {
            //_uiGameController.ButtonManager[type].onClick.AddListener(call);
        }
        public void RemoveListenerFromUIButton(UIGameController.ButtonTypes type, UnityEngine.Events.UnityAction call)
        {
            //_uiGameController.ButtonManager[type].onClick.RemoveListener(call);
        }

        public void SetEnableDiceDragAndDrop(bool value)
        {
            _pickupDragController.enabled = value;
        }

        public void TransitionToRollSetup()
        {
            TransitionToState(RoundState.RollSetup);
        }
        public void TransitionToDeclare()
        {
            TransitionToState(RoundState.Declare);
        }

        public void TransitionToChapeau()
        {
            // Implement as last
        }

        public void TransitionToReceived()
        {
            TransitionToState(RoundState.Received);
        }
    }
}
