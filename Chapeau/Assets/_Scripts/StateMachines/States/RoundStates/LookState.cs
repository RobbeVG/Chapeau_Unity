using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class LookState : RoundState
    {
        public LookState() : base(RoundStateType.Look) {}

        public override void Enter(RoundStateMachineController roundSM)
        {
            base.Enter(roundSM);
            if (roundSM.PreviousRoundState.Type == RoundStateType.Roll)
                roundSM.ChangeRoundStateNextFrame(RoundStateType.Declare);
        }

        public override void Exit(RoundStateMachineController roundSM)
        {
            base.Exit(roundSM);
        }
    }
}
