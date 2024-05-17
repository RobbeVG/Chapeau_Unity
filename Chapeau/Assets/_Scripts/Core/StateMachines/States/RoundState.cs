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

        public virtual IEnumerator Enter(RoundStateMachineController stateMachine) 
        {
            yield break; 
        }
        
        public virtual IEnumerator Exit(RoundStateMachineController stateMachine)
        {
            yield break;
        }
    }
}
