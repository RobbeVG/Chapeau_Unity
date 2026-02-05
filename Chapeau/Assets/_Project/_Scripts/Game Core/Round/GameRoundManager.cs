using Reflex.Extensions;
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
        public IRoundRolls RoundRolls => _context;


        //private RoundStateMachine _stateMachine;

        private void Awake()
        {
            _context = new RoundContext(
                Resources.Load<Roll>("Rolls/CurrentRoll"),
                Resources.Load<Roll>("Rolls/DeclaredRoll"),
                Resources.Load<Roll>("Rolls/PhysicalRoll")
            );
            _context.ListenToDiceRollEvents(_diceController);

            _stateMachine = new StateMachine<RoundState>(new Dictionary<RoundState, BaseState<RoundState>>()
            {
                { RoundState.RollSetup,  new RollSetupState(gameObject.scene.GetSceneContainer().Resolve<InputActionManager>()) },
                { RoundState.Declare,  new DeclareState(_context, _diceController) },
                { RoundState.Received,  new ReceivedState(_context, _diceController) },
                { RoundState.Chapeau,  new ChapeauState() },
            }, currentStateKey: RoundState.Declare);
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
        public void StartNewRound(int playerCount)
        {
            // Additional round-start logic
            _context.Clear();
            _context.PlayerTotal = playerCount;

            _stateMachine.ForcedNewCurrentState(RoundState.Declare, true);
            _stateMachine.Start();
        }

        /// <summary>
        /// Ends the current round and performs any necessary cleanup or finalization.
        /// This might involve resetting the state of dice, updating the UI, or finalizing round results.
        /// </summary>
        public void EndRound()
        {
            // Cleanup or finalize round
        }
    }
}
