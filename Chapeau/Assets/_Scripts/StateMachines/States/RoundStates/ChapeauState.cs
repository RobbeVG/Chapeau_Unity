using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class ChapeauState : RoundState
    {
        public ChapeauState() : base(RoundStateType.Chapeau) { }

        public override void Enter(RoundStateMachineController roundSM)
        {
            base.Enter(roundSM);
            if (roundSM.DeclaredRoll >= roundSM.PhysicalRoll)
                Debug.Log("You Lost");
            else
                Debug.Log("You Won");

            roundSM.ResetRound();
            roundSM.ChangeRoundStateNextFrame(RoundStateType.Roll);
        }
    }
}
