using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore
{
    /// <summary>
    /// A Generic Statemachine class that uses enum states for easy 
    /// </summary>
    /// <typeparam name="EState">An enum typed state key A</typeparam>
    /// <typeparam name="TStateMachine">The Derived statemachine name to ensure methods access derived state</typeparam>
    public abstract class StateMachine<EState, TStateMachine> : MonoBehaviour where EState : Enum where TStateMachine : StateMachine<EState, TStateMachine>
    {
        private Dictionary<EState, BaseState<EState, TStateMachine>> _states;
        protected bool _isTransitioningState = false;
        protected BaseState<EState, TStateMachine> _currentState;

        public EState CurrentStateKey { get => _currentState.StateKey; }
        protected IReadOnlyDictionary<EState, BaseState<EState, TStateMachine>> States => _states;

        public event Action OnStateChanged;

        protected StateMachine(Dictionary<EState, BaseState<EState, TStateMachine>> states, EState currentStateKey)
        {
            _states = states;
            Assert.IsTrue(_states.ContainsKey(currentStateKey), "The CurrentStateKey was not found in the given states for this State Machine");
            _currentState = _states[currentStateKey];
        }

        private void Start()
        {
            _currentState.EnterState((TStateMachine)this);
        }

        private void Update()
        {
            if (!_isTransitioningState)
            {
                EState nextStateKey = _currentState.GetNextState((TStateMachine)this);
                if (nextStateKey.Equals(_currentState.StateKey))
                    _currentState.UpdateState((TStateMachine)this);
                else
                    TransitionToState(nextStateKey);
            }
        }

        protected void TransitionToState(EState stateKey)
        {
            _isTransitioningState = true;
            _currentState.ExitState((TStateMachine)this);
            _currentState = States[stateKey];
            _currentState.EnterState((TStateMachine)this);
            _isTransitioningState = false;
            OnStateChanged?.Invoke();
            Debug.Log("New State: " + stateKey.ToString());
        }
    }
}
