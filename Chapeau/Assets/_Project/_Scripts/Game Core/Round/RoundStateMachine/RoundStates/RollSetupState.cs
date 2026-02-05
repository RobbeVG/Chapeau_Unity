using Reflex.Attributes;
using Seacore.Common;
using Seacore.Common.Statemachine;
using UnityEngine;

namespace Seacore.Game.RoundStates
{
    public sealed class RollSetupState : BaseState<RoundState>
    {
        private readonly InputActionManager _inputManager;

        public RollSetupState(InputActionManager inputActionManager) : base(RoundState.RollSetup) 
        {
            _inputManager = inputActionManager;
        }

        public override void EnterState() 
        {
            _inputManager.EnableDiceActions();
        }

        public override void ExitState() 
        {
            _inputManager.DisableDiceActions();
        }

        //TODO Decide if it stays or it goes
        public override RoundState GetNextState()
        {
            //Check if dice are rolled (physicaly)
            return RoundState.RollSetup;  
        }
    }
}
