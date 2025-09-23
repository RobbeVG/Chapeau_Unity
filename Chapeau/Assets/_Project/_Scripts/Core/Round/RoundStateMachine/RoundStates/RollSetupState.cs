using Seacore.Common;
using Seacore.Common.Statemachine;
using UnityEngine;

namespace Seacore.Game.RoundStates
{
    public sealed class RollSetupState : BaseState<RoundStateMachine.RoundState>
    {
        public RollSetupState() : base(RoundStateMachine.RoundState.RollSetup) 
        {}

        public override void EnterState() 
        {
            InputManager.Instance.SetDiceActions(true);
        }

        public override void ExitState() 
        {
            InputManager.Instance.SetDiceActions(false);
        }

        //TODO Decide if it stays or it goes
        public override RoundStateMachine.RoundState GetNextState()
        {
            //Check if dice are rolled (physicaly)
            return RoundStateMachine.RoundState.RollSetup;  
        }
    }
}
