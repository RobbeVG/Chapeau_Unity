using DG.Tweening;
using Seacore.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Seacore.Game
{
    /// <summary>
    /// Controls the behaviour of the dice.
    /// <para>Is responsible for hiding the dice, selecting the dice for rolling</para>
    /// </summary>
    [RequireComponent(typeof(DiceManager))]
    public class DiceController : MonoBehaviour
    {
        [SerializeField]
        private CircleController _circleController;
        [SerializeField]
        private DieVisualHandler _dieVisualHandler = null;

        private DiceManager _diceManager;
        private Roll _physicalRoll;
        public event Action OnAllDiceRolled;

        private void Awake()
        {
            if (_circleController == null)
                Debug.LogError("No circle Controller attached");

            _diceManager = GetComponent<DiceManager>();
            _physicalRoll = Resources.Load<Roll>("Rolls/PhysicalRoll");

        }

        private void Start()
        {
            _diceManager.SetDieEventDelegates(
                hoverEnter: (Die die) => { _diceManager.DiceContainers[die].Selectable.Select(); },
                hoverExit: (Die die) => { EventSystem.current.SetSelectedGameObject(null); },
                select: (Die die) => { _diceManager.DiceContainers[die].State |= DieState.Selecting; },
                deselect: (Die die) => { _diceManager.DiceContainers[die].State &= ~DieState.Selecting; }
                );
        }

        private void OnEnable()
        {
            InputManager IM = InputManager.Instance;
            if (IM)
            {
                IM.OnDieHoldExit += HandleDieDrop;
                IM.OnDieTapped += HandleDieTappedForRoll;
                IM.OnDiceActionsToggleChanged += SetDiceSelectables;
            }

            foreach (Die die in _diceManager.Dice) //Werkt niet want sommige dice moeten nog worden ingesteld
            {
                die.OnRolledValue += OnDieRolled;
            }
        }

        private void OnDisable()
        {
            InputManager IM = InputManager.Instance;
            if (IM)
            {
                IM.OnDieHoldExit -= HandleDieDrop;
                IM.OnDieTapped -= HandleDieTappedForRoll;
                IM.OnDiceActionsToggleChanged -= SetDiceSelectables;
            }

            foreach (Die die in _diceManager.Dice)
            {
                die.OnRolledValue -= OnDieRolled;
            }
        }

        public void RollDice()
        {
            Assert.IsNotNull(_diceManager, "DiceManager is not set in the DiceController while required.");

            //Roll the dice that are in the ToRoll state
            foreach (KeyValuePair<Die, DieInfo> dieInfoPair in _diceManager.DiceContainers)
            {
                Die die = dieInfoPair.Key;
                DieInfo info = dieInfoPair.Value;

                if (info.State.HasFlag(DieState.ToRoll))
                {
                    die.Roll();
                    _physicalRoll.ChangeValue(info.Index, die.DieValue);

                    info.State &= ~DieState.ToRoll; // Get rid of To Roll flag
                }
            }
            OnAllDiceRolled?.Invoke();
        }
        public void RevealDice()
        {
            foreach (Die die in _diceManager.Dice)
            {
                DieInfo dieInfo = _diceManager.DiceContainers[die];
                if (!dieInfo.State.HasFlag(DieState.Visible))
                {
                    dieInfo.State |= DieState.Visible;
                    _dieVisualHandler.RevealDie(dieInfo);
                }
            }
        }
        public void HideAllDie() 
        { 
            foreach (Die die in _diceManager.Dice)
                HideDie(die);
        }
        public void HideDie(Die die)
        {
            DieInfo dieInfo = _diceManager.DiceContainers[die];
            if (dieInfo.State.HasFlag(DieState.Inside))
            {
                dieInfo.State &= ~DieState.Visible;
                _dieVisualHandler.HideDieImmediatly(dieInfo);
            }
        }

        //Event handlers
        private void OnDieRolled(Die die)
        {
            die.Rigidbody.isKinematic = true;
            HideDie(die);
        }
        private void HandleDieDrop(Die die)
        {
            DieInfo info = _diceManager.DiceContainers[die];
            
            // When a die is dropped, we check if it is inside the circle and update its state accordingly
            DieState originalState = info.State;
            (Die[] inside, Die[] outside) = _diceManager.SplitDiceInfoBy(DieState.Inside);
            bool insideRollActive = inside.All(insideDie => _diceManager.DiceContainers[insideDie].State.HasFlag(DieState.ToRoll));
            bool inCircle = _circleController.IsPositionInCircle(die.transform.position);

            if (originalState.HasFlag(DieState.Inside) == inCircle)
                return; // No state change, exit early

            if (inCircle)
            {
                info.State |= DieState.Inside;
                if (originalState.HasFlag(DieState.ToRoll) != insideRollActive)
                    info.State ^= DieState.ToRoll; // If the die was toggled to roll, we toggle it off when it is dropped inside the circle and it came from outside
            }
            else
            {
                info.State &= ~DieState.Inside;
                if (originalState.HasFlag(DieState.ToRoll))
                    info.State ^= DieState.ToRoll; // If the die was toggled to roll, we toggle it off when it is dropped outside the circle and it came from inside
            }
        }
        private void HandleDieTappedForRoll(Die die)
        {
            (Die[] inside, Die[] outside) = _diceManager.SplitDiceInfoBy(DieState.Inside);
            DieInfo selectedDieInfo = _diceManager.DiceContainers[die];

            // Set dice to roll state depending if the die is inside or outside the circle

            // If the die is inside, we toggle all inside dice to the toggled selectedDie and detoggle the outside dice
            if (selectedDieInfo.State.HasFlag(DieState.Inside))
            {
                // Toggle all inside dice to the selected die's toggled state
                DieState selectedDieToggledRollState = (selectedDieInfo.State & ~DieState.Selecting) ^ DieState.ToRoll;
                foreach (Die d in inside)
                {
                    DieInfo info = _diceManager.DiceContainers[d];
                    
                    info.State = selectedDieToggledRollState; // Set the state to the selected die's state
                }
                // Detoggle outside dice
                foreach (Die d in outside)
                {
                    DieInfo info = _diceManager.DiceContainers[d];
                    info.State &= ~DieState.ToRoll; // Remove ToRoll flag
                }
            }
            // If the die is outside, we toggle only the selected die and detoggle the inside dice
            else
            {
                // Toggle the selected die to the toggled state
                selectedDieInfo.State ^= DieState.ToRoll; // Flip the ToRoll bit
                // Detoggle all inside dice
                foreach (Die d in inside)
                {
                    DieInfo info = _diceManager.DiceContainers[d];
                    info.State &= ~DieState.ToRoll; // Remove ToRoll flag
                }
            }
        }
        private void SetDiceSelectables(bool state)
        {
            foreach (KeyValuePair<Die, DieInfo> DiePair in _diceManager.DiceContainers)
            {
                DiePair.Value.Selectable.enabled = state;
            }
        }
    }
}
