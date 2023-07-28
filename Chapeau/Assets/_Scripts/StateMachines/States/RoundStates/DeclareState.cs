using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class DeclareState : RoundState
    {
        public DeclareState() : base(RoundStateType.Declare) {}

        public override void Exit(RoundStateMachineController roundSM)
        {
            base.Exit(roundSM);
            roundSM.CurrentRoll.ChangeValueTo(roundSM.DeclaredRoll);
        }
    }
}
