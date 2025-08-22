using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore.Common.Statemachine
{
    /// <summary>
    /// A Generic Statemachine class that uses enum states for easy 
    /// </summary>
    /// <typeparam name="EState">An enum typed state key A</typeparam>
    /// <typeparam name="TStateMachine">The Derived statemachine name to ensure methods access derived state</typeparam>
    
    [Serializable]
    public abstract class StateMachine<EState> where EState : Enum
    {
        private Dictionary<EState, BaseState<EState>> _states;
        [field: SerializeField, ReadOnly]
        protected bool _isTransitioningState = false;

        protected BaseState<EState> _currentState;

        public EState CurrentStateKey { get => _currentState.StateKey; }
        protected IReadOnlyDictionary<EState, BaseState<EState>> States => _states;

        public event Action<EState> OnStateEnter;
        public event Action<EState> OnStateExit;

        protected StateMachine(Dictionary<EState, BaseState<EState>> states, EState currentStateKey)
        {
            _states = states;
            Assert.IsTrue(_states.ContainsKey(currentStateKey), "The CurrentStateKey was not found in the given states for this State Machine");
            _currentState = _states[currentStateKey];
        }

        public void Start()
        {
            _currentState.EnterState();
            Debug.Log("Starting " + _currentState);
        }

        public void Update()
        {
            if (!_isTransitioningState)
            {
                EState nextStateKey = _currentState.GetNextState();
                if (nextStateKey.Equals(_currentState.StateKey))
                    _currentState.UpdateState();
                else
                    TransitionToState(nextStateKey);
            }
        }

        public void TransitionToState(EState stateKey)
        {
            _isTransitioningState = true;
            {
                ExitState();
                _currentState = States[stateKey];
                EnterState();
            }
            _isTransitioningState = false;

            Debug.Log("Transitioned to " + _currentState);
        }

        protected void ForcedNewCurrentState(EState stateKey,  bool invokePreviousExit = false)
        {
            if (invokePreviousExit)
                ExitState();

            _currentState = _states[stateKey];
        }

        private void EnterState()
        {
            _currentState.EnterState();
            //Async?
            OnStateEnter?.Invoke(_currentState.StateKey);
        }

        private void ExitState()
        {
            _currentState.ExitState();
            //Async?

            OnStateExit?.Invoke(_currentState.StateKey);
        }
    }
}
