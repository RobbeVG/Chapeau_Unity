using UnityEngine;

namespace Seacore
{
    public sealed class RollSetupState : BaseState<RoundStateMachine.RoundState, RoundStateMachine>
    {
        public RollSetupState() : base(RoundStateMachine.RoundState.RollSetup) {}

        public override void EnterState(RoundStateMachine stateMachine)
        {
            //stateMachine.AddListenerToUIButton(UIGameController.ButtonTypes.Roll, stateMachine.TransitionToDeclare);

            stateMachine.SetEnableDiceDragAndDrop(true);
        }

        public override void ExitState(RoundStateMachine stateMachine)
        {
            //stateMachine.RemoveListenerFromUIButton(UIGameController.ButtonTypes.Roll, stateMachine.TransitionToDeclare);

            stateMachine.SetEnableDiceDragAndDrop(false);
        }

        //TODO Decide if it stays or it goes
        public override RoundStateMachine.RoundState GetNextState(RoundStateMachine stateMachine)
        {
            //Check if dice are rolled (physicaly)
            return RoundStateMachine.RoundState.RollSetup;  
        }
    }
}
