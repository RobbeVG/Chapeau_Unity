using Seacore.Common.Statemachine;
using UnityEngine;

namespace Seacore
{
    public sealed class RollSetupState : BaseState<RoundStateMachine.RoundState>
    {
        GameObject _selectAndPickup = null;

        public RollSetupState(GameObject gameObjectSelectAndPickup) : base(RoundStateMachine.RoundState.RollSetup) 
        {
            _selectAndPickup = gameObjectSelectAndPickup;
        }

        public override void EnterState() 
        {
            _selectAndPickup.SetActive(true);
        }

        public override void ExitState() 
        {
            _selectAndPickup.SetActive(false);
        }

        //TODO Decide if it stays or it goes
        public override RoundStateMachine.RoundState GetNextState()
        {
            //Check if dice are rolled (physicaly)
            return RoundStateMachine.RoundState.RollSetup;  
        }
    }
}
