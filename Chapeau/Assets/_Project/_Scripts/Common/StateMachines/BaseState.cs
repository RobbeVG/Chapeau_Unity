using System;
namespace Seacore.Common.Statemachine
{
    /// <summary>
    /// Represents the base class for a state in a state machine.
    /// </summary>
    /// <typeparam name="EState">The enumeration representing the states.</typeparam>
    /// <typeparam name="TStateMachine">The type of the state machine this state belongs to.</typeparam>
    [Serializable]
    public abstract class BaseState<EState> where EState : Enum
    {
        /// <summary>
        /// Gets the key representing this state.
        /// </summary>
        public EState StateKey { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseState{EState, TStateMachine}"/> class with the specified state key.
        /// </summary>
        /// <param name="key">The key representing this state.</param>
        public BaseState(EState key)
        {
            StateKey = key;
        }

        /// <summary>
        /// Converts a <see cref="BaseState{T}"/> instance to its corresponding <see cref="EState"/> value.
        /// </summary>
        /// <param name="state">The <see cref="BaseState{EState}"/> instance to convert. Must not be <c>null</c>.</param>
        public static explicit operator EState(BaseState<EState> state) => state.StateKey;

        /// <summary>
        /// Called when the state is entered.
        /// </summary>
        /// <param name="stateMachine">The state machine that this state is part of.</param>
        public abstract void EnterState();

        /// <summary>
        /// Called every frame while the state is active. Can be overridden by derived classes.
        /// </summary>
        /// <param name="stateMachine">The state machine that this state is part of.</param>
        public virtual void UpdateState()
        {
            // This method can be overridden by derived classes to handle updates in the current state.
        }

        /// <summary>
        /// Called when the state is exited.
        /// </summary>
        /// <param name="stateMachine">The state machine that this state is part of.</param>
        public abstract void ExitState();

        /// <summary>
        /// Determines the next state to transition to from this state.
        /// </summary>
        /// <param name="stateMachine">The state machine that this state is part of.</param>
        /// <returns>The key of the next state to transition to.</returns>
        public abstract EState GetNextState();

        public override string ToString()
        {
            return $"State: {StateKey}";
        }
    }
}
