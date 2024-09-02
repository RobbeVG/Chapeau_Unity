using Seacore.Common.Statemachine;

namespace Seacore
{
    public class ReceivedState : BaseState<RoundStateMachine.RoundState>
    {
        private readonly RoundContext _roundContext;
        private readonly DiceController _diceController;

        public ReceivedState(RoundContext context, DiceController diceController) 
            : base(RoundStateMachine.RoundState.Received) 
        {
            _roundContext = context;
            _diceController = diceController;
        }

        public override void EnterState()
        {
            _roundContext.CurrentRoll.ChangeValueTo(_roundContext.DeclaredRoll);
            _diceController.HideInsideDiceImmediatly();
        }

        public override void ExitState() {}

        public override RoundStateMachine.RoundState GetNextState()
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
