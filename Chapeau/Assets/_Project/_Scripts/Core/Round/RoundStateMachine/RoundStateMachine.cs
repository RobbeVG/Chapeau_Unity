using System;
using System.Collections.Generic;
using UnityEngine;
using Seacore.Common.Statemachine;

namespace Seacore
{
    /// <summary>
    /// A child statemachine that specifically is used to progress a round
    /// </summary>
    public sealed class RoundStateMachine : StateMachine<RoundStateMachine.RoundState>
    {
        /// <summary>
        /// Enum representing the possible states of a round.
        /// </summary>
        [Serializable]
        public enum RoundState
        {
            RollSetup, Declare, Received, Chapeau
        }

        public RoundStateMachine(RoundContext context, DiceRoller diceRoller, DiceController diceController, GameObject gameObjectSelectAndPickup)
            : base(new Dictionary<RoundState, BaseState<RoundState>>()
            {
                { RoundState.RollSetup,  new RollSetupState(gameObjectSelectAndPickup) },
                { RoundState.Declare,  new DeclareState(context, diceRoller) },
                { RoundState.Received,  new ReceivedState(context, diceController) },
                { RoundState.Chapeau,  new ChapeauState() },
            }, currentStateKey: RoundState.Declare)
        { }

        /// <summary>
        /// Transition to the state given.
        /// </summary>
        /// <param name="state">Of type <see cref="RoundState"/></param>
        public new void TransitionToState(RoundState state)
        {
            base.TransitionToState(state);
        }

        public void Reset()
        {
            ForcedNewCurrentState(RoundState.Declare, true);
        }
    }
}
