using Seacore.Common.Statemachine;

namespace Seacore.Game.RoundStates
{
    public class ChapeauState : BaseState<RoundState>
    {
        public ChapeauState() : base(RoundState.Chapeau) { }

        public override void EnterState() { }

        public override void ExitState() { }

        public override RoundState GetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}
