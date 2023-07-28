using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore
{
    public class RoundManager : MonoBehaviour
    {
        private RoundState currentState = null;
        [Header("Round States")]

        [Header("Rolls")]
        [SerializeField]
        private Roll physicalRoll;
        [SerializeField]
        private Roll currentRoll;
        [SerializeField]
        private Roll declaredRoll;


        public Roll PhysicalRoll { get { return physicalRoll; } }
        public Roll CurrentRoll { get { return currentRoll; } }
        public Roll DeclaredRoll { get { return declaredRoll; } }
        public DiceManager DiceManager { get; private set; }
        //public GameStateManager GameManager { get; private set; }
        public UIManager UIManager { get; private set; }
        public GrabController Grabber { get; private set; }

        void Start()
        {
            DiceManager = GetComponent<DiceManager>();
            UIManager = GetComponent<UIManager>();
            Grabber = GetComponent<GrabController>();
            if (DiceManager == null)
                Debug.LogError("Not all managers found");

            physicalRoll.Clear();
            currentRoll.Clear();
            declaredRoll.Clear();

            //SwitchToState(roll);
        }

        void Update()
        {
            //currentState.Update(this);
        }

        public void SwitchToState(RoundState state)
        {
            Assert.IsNotNull(state);

            //currentState?.Exit(this);
            currentState = state;
            //currentState.Enter(this);
        }
    }
}
/*
       private Coroutine _runningCoroutineState;

       private bool _selectDice = false;

       //public event Action<GameState> OnGameStateChanged;

       public void Start()
       {
           physicalRoll.Clear();
           currentRoll.Clear();
           UpdateGameState(state); //Can be set in the inspector
       }

       public void CheckIfDeclaredIsHigherThanPrevious() //Is linked to pass on button
       {
           declaredRoll.CalculateResult(); //Do this on seperate thread or something so that we can enable/disable up button
           if (declaredRoll > currentRoll)
           {
               currentRoll.ChangeValueTo(declaredRoll);
               currentRoll.Sort();
               UpdateGameState(GameState.SelectAction);
               declare.SetActive(false);
               declaredRoll.Clear();
           }
       }

       //Button press
       public void AfterLookRoll()
       {
           StopCoroutine(Look());
           UpdateGameState(GameState.Roll);
       }
       //Button press
       public void ActionBlind()
       {
           DoAction(Action.Blind);
       }
       //Button press
       public void ActionLook()
       {
           DoAction(Action.Look);
       }
       //Button press
       public void ActionChapeau()
       {
           DoAction(Action.Chapeau);
       }
       private void DoAction(Action action)
       {
           actions.SetActive(false);
           switch (action)
           {
               case Action.Look:
                   StartCoroutine(Look());
                   break;
               case Action.Blind:
                   UpdateGameState(GameState.Declare);
                   break;
               case Action.Chapeau:
                   StartCoroutine(Chapeau());
                   break;
           }
       }

       private IEnumerator DoRoll()
       {
           Log.gameManager.Log("Started Task DoRoll");
           yield return StartCoroutine(diceManager.Roll()); 
           physicalRoll.Sort();
           physicalRoll.CalculateResult();   
           Log.gameManager.Log("Finished Task DoRoll");

           UpdateGameState(GameState.Declare);
       }

       private IEnumerator PassOn() 
       {
           Log.gameManager.Log("Started Task PassOn");
           yield return null;
           actions.SetActive(true);
           Log.gameManager.Log("Finished Task PassOn");
       }

       private IEnumerator Look()
       {
           Log.gameManager.Log("Started Action Look");
           rollUi.SetActive(true);
           while (true)
           {
               diceManager.TrySelectDice();
               yield return null;
           }
           Log.gameManager.Log("Finished Action Look");
       }

       private IEnumerator Chapeau()
       {
           Log.gameManager.Log("Started Action Chapeau");

           yield return null;

           Log.gameManager.Log("Finished Action Chapeau");
       }

       private void UpdateGameState(GameState newState)
       {
           if (_runningCoroutineState != null)
           {
               StopCoroutine(_runningCoroutineState);
               _runningCoroutineState = null;
           }
           state = newState;
           switch (newState)
           {
               case GameState.Roll:
                   _runningCoroutineState = StartCoroutine(DoRoll());
                   break;
               case GameState.Declare:
                   declare.SetActive(true);
                   break;
               case GameState.SelectAction:
                   _runningCoroutineState = StartCoroutine(PassOn());
                   break;
               default:
                   break;
           }
       } 

 */