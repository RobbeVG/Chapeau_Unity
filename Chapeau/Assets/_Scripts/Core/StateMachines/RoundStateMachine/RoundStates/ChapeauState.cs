using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class ChapeauState : BaseState<RoundStateMachine.RoundState, RoundStateMachine>
    {
        public ChapeauState() : base(RoundStateMachine.RoundState.Chapeau) { }

        public override void EnterState(RoundStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState(RoundStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }

        public override RoundStateMachine.RoundState GetNextState(RoundStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }

        //public override IEnumerator Enter(RoundStateMachine roundSM)
        //{
        //    base.Enter(roundSM);
        //    if (roundSM.DeclaredRoll >= roundSM.PhysicalRoll)
        //        Debug.Log("You Lost");
        //    else
        //        Debug.Log("You Won");

        //    roundSM.ResetRound();
        //    roundSM.ChangeRoundState(RoundStateType.Roll);
        //    yield break;
        //}
    }
}
