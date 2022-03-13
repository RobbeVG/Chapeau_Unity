using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public enum GameState { Roll, Declare, Action, End }

    [RequireComponent(typeof(DiceManager))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        public GameState state = GameState.Roll;
        [SerializeField]
        private Chapeau chapeau;

        [SerializeField]
        private Roll physicalRoll;
        [SerializeField]
        private Roll fictionalRoll;
       
        private DiceManager diceManager;
        private Coroutine _runningCoroutineState;

        //public event Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            diceManager = GetComponent<DiceManager>();
        }

        public IEnumerator DoRoll()
        {
            Debug.Log("Started Task DoRoll");
            yield return StartCoroutine(chapeau.Close());
            yield return StartCoroutine(diceManager.Roll()); 
            physicalRoll.Sort();
            physicalRoll.CalculateResult();   
            Debug.Log("Finished Task DoRoll");
            UpdateGameState(GameState.Declare);
        }

        public IEnumerator DoDeclareRoll()
        {
            yield return StartCoroutine(chapeau.Open());
            //If start of round choose direction

            //UI
            UpdateGameState(GameState.Action);
        }

        public void Start()
        {
            UpdateGameState(state);
        }

        public void UpdateGameState(GameState newState)
        {
            if (_runningCoroutineState != null)
                StopCoroutine(_runningCoroutineState);
            state = newState;
            switch (newState)
            {
                case GameState.Roll:
                    _runningCoroutineState = StartCoroutine(DoRoll());
                    break;
                case GameState.Declare:
                    _runningCoroutineState = StartCoroutine(DoDeclareRoll());
                    break;
                case GameState.Action:
                    break;
                case GameState.End:
                    break;
                default:
                    break;
            }
        }
    }
}
