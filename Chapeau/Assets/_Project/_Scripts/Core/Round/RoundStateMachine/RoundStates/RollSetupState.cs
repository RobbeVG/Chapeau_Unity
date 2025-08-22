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
            PlayerInputManager.Instance.Pointing = true;
            PlayerInputManager.Instance.Grabbing = true;
        }

        public override void ExitState() 
        {
            PlayerInputManager.Instance.Pointing = false;
            PlayerInputManager.Instance.Grabbing = false;  
        }

        //TODO Decide if it stays or it goes
        public override RoundStateMachine.RoundState GetNextState()
        {
            //Check if dice are rolled (physicaly)
            return RoundStateMachine.RoundState.RollSetup;  
        }
    }
}
