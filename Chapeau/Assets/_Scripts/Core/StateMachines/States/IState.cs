using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    /// <summary>
    /// Interface for States
    /// </summary>
    /// <typeparam name="T"> Is of type stateMachine, you cannot have this in private field. Otherwise we cannot generate states all at once.</typeparam>
    public interface IState<T> where T : StateMachine<T>
    {
        IEnumerator Enter(T stateMachine);
        IEnumerator Exit(T stateMachine);
    }
}
