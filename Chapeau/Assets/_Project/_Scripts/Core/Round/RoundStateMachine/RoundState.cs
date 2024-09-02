using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Seacore.Common.Statemachine;

namespace Seacore
{
    public class RoundState : BaseState<RoundStateMachine.RoundState>
    {
        private static readonly RoundContext _roundContext;
        public RoundState(RoundStateMachine.RoundState key) : base(key)
        {
        }

        public override void EnterState() { }

        public override void ExitState() { }

        public override RoundStateMachine.RoundState GetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}


