using UnityEngine;
using Seacore.Common.Statemachine;

namespace Seacore
{
    public class DeclareState : BaseState<RoundStateMachine.RoundState>
    {
        private readonly Roll _physicalRoll;
        private readonly DiceRoller _diceRoller;

        public DeclareState(RoundContext context, DiceRoller diceRoller) 
            : base(RoundStateMachine.RoundState.Declare) 
        {
            _physicalRoll = context.PhysicalRoll;
            _diceRoller = diceRoller;
        }
        // Create listener to alert other players if he looked

        public override void EnterState()
        {
            //Remove previous onClick events if there
            //stateMachine.AddListenerToUIButton(UIGameController.ButtonTypes.DeclareConfirm, stateMachine.TransitionToReceived);

            _diceRoller.RollDice(); //_diceController.HideInsideDiceImmediatly(); //Happens automatically when die is rolled inside!
            _physicalRoll.CalculateResult();

            //Debug.Log(stateMachine.PhysicalRoll.ToString());
        }

        public override void ExitState()
        {
            //stateMachine.RemoveListenerFromUIButton(UIGameController.ButtonTypes.DeclareConfirm, stateMachine.TransitionToReceived);

            //Do not set here stateMachine.CurrentRoll.ChangeValueTo(stateMachine.DeclaredRoll); -> IN receive state because you can direct pass ON

        }

        public override RoundStateMachine.RoundState GetNextState()
        {
            return RoundStateMachine.RoundState.Declare;
        }

        //public override IEnumerator Exit(RoundStateMachine roundSM)
        //{ 
        //    roundSM.CurrentRoll.ChangeValueTo(roundSM.DeclaredRoll);
        //    yield break;
        //}
    }
}
