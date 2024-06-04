using System.Collections;
using UnityEngine.Assertions;
using System;

namespace Seacore
{
    public abstract class BaseState<EState, TStateMachine> where EState : Enum where TStateMachine : StateMachine<EState, TStateMachine>
    {
        public BaseState(EState key)
        {
            StateKey = key;
        }

        public EState StateKey { get; private set; }

        public abstract void EnterState(TStateMachine stateMachine);
        public void UpdateState(TStateMachine stateMachine) { }
        public abstract void ExitState(TStateMachine stateMachine);
        public abstract EState GetNextState(TStateMachine stateMachine);
    }
}
