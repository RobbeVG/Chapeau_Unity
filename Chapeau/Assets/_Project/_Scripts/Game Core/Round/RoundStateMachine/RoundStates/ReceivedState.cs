using Seacore.Common.Statemachine;
using Seacore.Game.RoundStates;
using UnityEngine;

namespace Seacore.Game
{
    public class ReceivedState : BaseState<RoundState>
    {
        private readonly RoundContext _roundContext;
        private readonly DiceController _diceController;

        bool counterclockwise = true;

        public ReceivedState(RoundContext context, DiceController diceController) 
            : base(RoundState.Received) 
        {
            _roundContext = context;
            _diceController = diceController;
        }

        public override void EnterState()
        {
            _roundContext.CurrentRoll.ChangeValueTo(_roundContext.DeclaredRoll);

            if (counterclockwise) 
            {                 
                _roundContext.CurrentPlayer = _roundContext.CurrentPlayer.Previous ?? _roundContext.Players.Last;
            }
            else
            {
                _roundContext.CurrentPlayer = _roundContext.CurrentPlayer.Next ?? _roundContext.Players.First;
            }

            _diceController.HideAllDie();
        }

        public override void ExitState() {}

        public override RoundState GetNextState()
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
