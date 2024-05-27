using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public enum RollLocation : byte
    {
        Inside, Outside
    }

    [RequireComponent(typeof(DiceManager))]
    public class DiceRoller : MonoBehaviour
    {
        [SerializeField]
        private Roll physicalRoll;
        [SerializeField]
        private RollLocation rollLocation = RollLocation.Inside;

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

                if (info.Location == rollLocation)
                {
                    die.Roll();
                    physicalRoll.ChangeValue(info.Index, die.DieValue);
                }
            }
        }
    }
}
