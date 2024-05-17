using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class RoundStateMachineController : StateMachine<RoundStateMachineController>
    {
        private static Dictionary<RoundStateType, RoundState> RoundStateHolder = new Dictionary<RoundStateType, RoundState>()
        {
            { RoundStateType.Roll,  new RollState() },
            { RoundStateType.Look,  new LookState() },
            { RoundStateType.Declare,  new DeclareState() },
            { RoundStateType.PassOn,  new PassOnState() },
            { RoundStateType.InOut,  new InOutState() },
            { RoundStateType.Chapeau,  new ChapeauState() },
        };
        public RoundStateMachineController() 
            : base(RoundStateHolder[RoundStateType.Roll]) {}

        [SerializeField]
        private DiceManager _diceManager = null;

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
        public DiceManager DiceManager { get { return _diceManager; } }

        public RoundState CurrentRoundState { get { return currentState as RoundState; } }
        public RoundState PreviousRoundState { get { return previousState as RoundState; } }
        public uint AmountRolled { get; private set; } = 1;

        public event Action<RoundStateType> OnRoundStateChange;


        private new void Start()
        {
            ResetRound();
            base.Start();
        }

        public void ResetRound()
        {
            physicalRoll.Clear();
            currentRoll.Clear();
            declaredRoll.Clear();
        }

        public RoundState GetCurrentRoundState()
        {
            return CurrentRoundState;
        }

        public void ChangeRoundState(RoundStateType roundStateType)
        {
            if (roundStateType == RoundStateType.Roll)
                AmountRolled += 1;
            StartCoroutine(ChangeRoundStateAndFireEvent(roundStateType));
        } 
        private IEnumerator ChangeRoundStateAndFireEvent(RoundStateType roundStateType)
        {
            yield return StartCoroutine(SetState(RoundStateHolder[roundStateType]));
            OnRoundStateChange?.Invoke(CurrentRoundState.Type);
        }
    }
}
