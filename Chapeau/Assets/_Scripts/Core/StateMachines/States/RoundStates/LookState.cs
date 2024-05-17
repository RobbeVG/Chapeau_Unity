using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class LookState : RoundState
    {
        public LookState() : base(RoundStateType.Look) {}

        public override IEnumerator Enter(RoundStateMachineController roundSM)
        {
            if (roundSM.PreviousRoundState.Type == RoundStateType.Roll)
                roundSM.ChangeRoundState(RoundStateType.Declare);
            yield break;
        }
    }
}
