using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class RollDeclareState : BaseState<RoundStateMachine.RoundState, RoundStateMachine>
    {
        public RollDeclareState() : base(RoundStateMachine.RoundState.Declare) { }

        public int AmountRolled { get; private set; } = 0;

        // Create listener to alert other players if he looked

        public override void EnterState(RoundStateMachine stateMachine)
        {
            //Remove previous onClick events if there
            //stateMachine.AddListenerToUIButton(UIGameController.ButtonTypes.DeclareConfirm, stateMachine.TransitionToReceived);

            stateMachine.DiceRoller.RollDice();
            AmountRolled++;
            stateMachine.PhysicalRoll.CalculateResult();
            Debug.Log(stateMachine.PhysicalRoll.ToString());
        }

        public override void ExitState(RoundStateMachine stateMachine)
        {
            //stateMachine.RemoveListenerFromUIButton(UIGameController.ButtonTypes.DeclareConfirm, stateMachine.TransitionToReceived);

            //Do not set here stateMachine.CurrentRoll.ChangeValueTo(stateMachine.DeclaredRoll); -> IN receive state because you can direct pass ON

        }

        public override RoundStateMachine.RoundState GetNextState(RoundStateMachine stateMachine)
        {
            //Check if passOn button is pressed.
            return RoundStateMachine.RoundState.Declare;
        }

        //public override IEnumerator Exit(RoundStateMachine roundSM)
        //{ 
        //    roundSM.CurrentRoll.ChangeValueTo(roundSM.DeclaredRoll);
        //    yield break;
        //}
    }
}
