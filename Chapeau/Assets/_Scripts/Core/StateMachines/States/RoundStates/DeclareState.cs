using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class DeclareState : RoundState
    {
        public DeclareState() : base(RoundStateType.Declare) {}

        public override IEnumerator Exit(RoundStateMachineController roundSM)
        { 
            roundSM.CurrentRoll.ChangeValueTo(roundSM.DeclaredRoll);
            yield break;
        }
    }
}
