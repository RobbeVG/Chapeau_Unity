using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore
{
    [DisallowMultipleComponent]
    public abstract class StateMachine<T> : MonoBehaviour where T : StateMachine<T>
    {
        protected IState<T> previousState = new NullState<T>();
        protected IState<T> currentState; //Can never be null.

        protected StateMachine(IState<T> startState)
        {
            currentState = startState;
        }

        protected void Start()
        {
            currentState.Enter(this as T);
        }

        //protected void Update()
        //{
        //    //currentState.
        //}

        //protected void OnDestroy()
        //{
        //    //currentState.Exit(); //->Needed?
        //}

        protected void SetState(IState<T> newState)
        {
            Assert.IsNotNull(newState, "New State is null and cannot be set");
            Debug.Log("Changed to State: " + newState.ToString());

            currentState.Exit(this as T); //Always a state (NullState)
            previousState = currentState;
            currentState = newState;
            currentState.Enter(this as T);
        }

        public void ChangeToPreviousState()
        {
            SetState(previousState);
        }
    }
}
