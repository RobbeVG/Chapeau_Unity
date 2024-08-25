using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public enum DiceLocation : byte
    {
        Inside, Outside
    }

    [RequireComponent(typeof(DiceManager))]
    public class DiceRoller : MonoBehaviour
    {
        [SerializeField]
        private Roll physicalRoll;

        private DiceManager diceManager;

        private void Awake()
        {
            diceManager = GetComponent<DiceManager>(); //Component is required
        }

        /// <summary>
        /// Rolls the dice that are located at the current roll location.
        /// </summary>
        public void RollDice()
        {
            if (diceManager == null)
                return;

            foreach (KeyValuePair<Die, DieInfo> dieInfoPair in diceManager.DiceContainers)
            {
                Die die = dieInfoPair.Key;
                DieInfo info = dieInfoPair.Value;

                if (info.State.HasFlag(DieState.ToRoll))
                {
                    die.Roll();
                    physicalRoll.ChangeValue(info.Index, die.DieValue);

                    info.State &= ~DieState.ToRoll; // Get rid of To Roll flag
                }
            }
        }
    }
}
