using Seacore.Common.Statemachine;
using Seacore.Game.RoundStates;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore.Game
{
    /// <summary>
    /// Manages the logic and coordination of a round in the game.
    /// This includes starting, updating, and ending the round, as well as interacting with
    /// various game components such as dice rollers and controllers.
    /// </summary>
    public class GameRoundManager : MonoBehaviour
    {
        private RoundContext _context;
        private StateMachine<RoundState> _stateMachine;

        [SerializeField]
        private DiceController _diceController = null;

        public IStateMachineEvents<RoundState> StateMachineEvents => _stateMachine;
        public IStateMachineTransitions<RoundState> StateMachineTransitions => _stateMachine;
        public RoundState CurrentState => _stateMachine.CurrentStateKey;


        public RoundContext Context { get { return _context; } }


        //private RoundStateMachine _stateMachine;

        private void Awake()
        {
            _context = new RoundContext(
                Resources.Load<Roll>("Rolls/CurrentRoll"),
                Resources.Load<Roll>("Rolls/DeclaredRoll"),
                Resources.Load<Roll>("Rolls/PhysicalRoll")
            );


            _stateMachine = new StateMachine<RoundState>(new Dictionary<RoundState, BaseState<RoundState>>()
            {
                { RoundState.RollSetup,  new RollSetupState(Reflex.Core.Container.RootContainer.Resolve<InputActionManager>()) },
                { RoundState.Declare,  new DeclareState(_context, _diceController) },
                { RoundState.Received,  new ReceivedState(_context, _diceController) },
                { RoundState.Chapeau,  new ChapeauState() },
            }, currentStateKey: RoundState.Declare);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameRoundManager"/> class with the specified context.
        /// </summary>
        /// <param name="context">The <see cref="RoundContext"/> containing dependencies for the round.</param>
        private void Start()
        {
            _context.Clear();
            StartNewRound();
        }

        private void OnEnable()
        {
            _diceController.OnAllDiceRolled += _context.IncrementAmountRolled;
        }

        private void OnDisable()
        {
            _diceController.OnAllDiceRolled -= _context.IncrementAmountRolled;
        }

        private void OnDestroy()
        {
            _context.Clear();
        }

        /// <summary>
        /// Starts a new round by initializing or resetting round-specific elements.
        /// This method might involve setting up the dice roller, initializing dice controllers,
        /// and preparing any other necessary components.
        /// </summary>
        public void StartNewRound()
        {
            _stateMachine.Start();

            // Additional round-start logic
        }

        /// <summary>
        /// Ends the current round and performs any necessary cleanup or finalization.
        /// This might involve resetting the state of dice, updating the UI, or finalizing round results.
        /// </summary>
        public void EndRound()
        {
            // Cleanup or finalize round
        }

        /// <summary>
        /// Resets the state of the round, clearing or reinitializing round-specific data and components.
        /// This method is typically called at the beginning of a new round or when restarting the game.
        /// </summary>
        public void ResetRound()
        {
            _context.Clear();
            _stateMachine.ForcedNewCurrentState(RoundState.Declare, true);
        }

        // Additional methods to manage round logic can be added here.
    }
}
