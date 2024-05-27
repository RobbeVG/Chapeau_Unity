using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public sealed class RollState : RoundState
    {
        public RollState() : base(RoundStateType.Roll) {}

        public override IEnumerator Enter(RoundStateMachineController roundSM)
        {
            roundSM.DiceRoller.RollDice();
            roundSM.PhysicalRoll.CalculateResult();
            Debug.Log(roundSM.PhysicalRoll.ToString());
            yield break;
        }
    }
}
