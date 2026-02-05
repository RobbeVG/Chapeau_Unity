using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore.Common.Statemachine
{
    /// <summary>
    /// Defines events that are raised when a state machine enters or exits a state.
    /// </summary>
    /// <remarks>Implementations of this interface allow subscribers to respond to state transitions by
    /// handling the corresponding events. Event handlers receive the state that is being entered or exited. This
    /// interface is typically used to observe or react to state changes in a state machine without directly controlling
    /// its transitions.</remarks>
    /// <typeparam name="EState">The enumeration type representing the possible states of the state machine.</typeparam>
    public interface IStateMachineEvents<EState> where EState : Enum
    {
        public event Action<EState> OnStateEnter;
        public event Action<EState> OnStateExit;
    }

    /// <summary>
    /// Defines methods for managing state transitions in a state machine using an enumeration type as state keys.
    /// </summary>
    /// <remarks>Implementations of this interface allow for controlled transitions between states, including
    /// both standard and forced transitions. The interface is generic to support any enum-based state
    /// representation.</remarks>
    /// <typeparam name="EState">The enumeration type that represents the possible states of the state machine.</typeparam>
    public interface IStateMachineTransitions<EState> where EState : Enum
    {
        public void TransitionToState(EState stateKey);
        public void ForcedNewCurrentState(EState stateKey, bool invokePreviousExit = false);
    }

    /// <summary>
    /// A Generic Statemachine class that uses enum states for easy 
    /// </summary>
    /// <typeparam name="EState">An enum typed state key A</typeparam>
    /// <typeparam name="TStateMachine">The Derived statemachine name to ensure methods access derived state</typeparam>
    [Serializable]
    public class StateMachine<EState> : IStateMachineTransitions<EState>, IStateMachineEvents<EState> where EState : Enum
    {
        private Dictionary<EState, BaseState<EState>> _states;

        [field: SerializeField, ReadOnly]
        protected bool _isTransitioningState = false;

        protected BaseState<EState> _currentState;

        public EState CurrentStateKey { get => _currentState.StateKey; }

        public event Action<EState> OnStateEnter;
        public event Action<EState> OnStateExit;

        public StateMachine(Dictionary<EState, BaseState<EState>> states, EState currentStateKey)
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
                _currentState = _states[stateKey];
                EnterState();
            }
            _isTransitioningState = false;

            Debug.Log("Transitioned to " + _currentState);
        }

        public void ForcedNewCurrentState(EState stateKey,  bool invokePreviousExit = false)
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
