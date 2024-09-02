using Seacore.Common.Statemachine;


namespace Seacore
{
    public class ChapeauState : BaseState<RoundStateMachine.RoundState>
    {
        public ChapeauState() : base(RoundStateMachine.RoundState.Chapeau) { }

        public override void EnterState() { }

        public override void ExitState() { }

        public override RoundStateMachine.RoundState GetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}
