using System;
using System.Collections.Generic;
using UnityEngine;
using Seacore.Common.Statemachine;
using Seacore.Game.RoundStates;
using Seacore.Common;

namespace Seacore.Game
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

        public RoundStateMachine(RoundContext context, DiceController diceController)
            : base(new Dictionary<RoundState, BaseState<RoundState>>()
            {
                { RoundState.RollSetup,  new RollSetupState() },
                { RoundState.Declare,  new DeclareState(context, diceController) },
                { RoundState.Received,  new ReceivedState(context, diceController) },
                { RoundState.Chapeau,  new ChapeauState() },
            }, currentStateKey: RoundState.Declare)
        { }

        public void Reset()
        {
            ForcedNewCurrentState(RoundState.Declare, true);
        }
    }
}
