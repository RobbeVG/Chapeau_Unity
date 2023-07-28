using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public sealed class RollState : RoundState
    {
        public RollState() : base(RoundStateType.Roll) {}

        public override void Enter(RoundStateMachineController roundSM)
        {
            base.Enter(roundSM);
            roundSM.DiceManager.Roll();
            roundSM.PhysicalRoll.CalculateResult();
            Debug.Log(roundSM.PhysicalRoll.ToString());
        }
    }
}
