using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class NullState<T> : IState<T> where T : StateMachine<T>
    {
        public void Enter(T stateMachine)
        {
        }

        public void Exit(T stateMachine)
        {
        }
    }
}
