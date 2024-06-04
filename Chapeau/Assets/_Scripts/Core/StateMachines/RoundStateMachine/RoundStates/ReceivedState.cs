using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class ReceivedState : BaseState<RoundStateMachine.RoundState, RoundStateMachine>
    {
        public ReceivedState() : base(RoundStateMachine.RoundState.Received) { }

        public override void EnterState(RoundStateMachine stateMachine)
        {
            //stateMachine.AddListenerToUIButton(UIGameController.ButtonTypes.Reveal, stateMachine.TransitionToRollSetup);
            //stateMachine.AddListenerToUIButton(UIGameController.ButtonTypes.Roll, stateMachine.TransitionToDeclare);
            //stateMachine.AddListenerToUIButton(UIGameController.ButtonTypes.DeclareConfirm, stateMachine.TransitionToReceived);

            stateMachine.CurrentRoll.ChangeValueTo(stateMachine.DeclaredRoll);
        }

        public override void ExitState(RoundStateMachine stateMachine)
        {
            //stateMachine.RemoveListenerFromUIButton(UIGameController.ButtonTypes.Reveal, stateMachine.TransitionToRollSetup);
            //stateMachine.RemoveListenerFromUIButton(UIGameController.ButtonTypes.Roll, stateMachine.TransitionToDeclare);
            //stateMachine.RemoveListenerFromUIButton(UIGameController.ButtonTypes.DeclareConfirm, stateMachine.TransitionToReceived);
        }

        public override RoundStateMachine.RoundState GetNextState(RoundStateMachine stateMachine)
        {
            return StateKey;
        }

        //public override IEnumerator Enter(RoundStateMachine roundSM)
        //{
        //    if (roundSM.PreviousRoundState.Type == RoundStateType.Roll)
        //        roundSM.ChangeRoundState(RoundStateType.Declare);
        //    yield break;
        //}
    }
}
