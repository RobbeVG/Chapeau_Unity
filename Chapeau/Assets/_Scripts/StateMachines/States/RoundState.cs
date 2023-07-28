using System.Collections;
using UnityEngine.Assertions;
using System;

namespace Seacore
{
    [Serializable]
    public enum RoundStateType
    {
        Roll, Look, Declare, InOut, PassOn, Chapeau
    }

    public abstract class RoundState : IState<RoundStateMachineController>
    {
        public RoundStateType Type { get; private set; }

        protected RoundState(RoundStateType type) 
        {
            Type = type; 
        }

        public virtual void Enter(RoundStateMachineController roundSM)
        {
            Assert.IsNotNull(roundSM);
        }

        public virtual void Update(RoundStateMachineController roundSM)
        {
            Assert.IsNotNull(roundSM);
        }

        public virtual void Exit(RoundStateMachineController roundSM)
        {
            Assert.IsNotNull(roundSM);
        }
    }
}
