using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class NullState<T> : IState<T> where T : StateMachine<T>
    {
        public IEnumerator Enter(T stateMachine)
        {
            yield break;
        }

        public IEnumerator Exit(T stateMachine)
        {
            yield break;
        }
    }
}
