using System;
using UnityEngine;

namespace Seacore
{
    namespace RoundStates
    {
        [Serializable]
        public class Look : RoundState
        {
            public override void Enter(RoundManager rm)
            {
                // Show Dice
                rm.UIManager.background.SetActive(false);

                if (rm.DeclaredRoll.IsEmpty())
                    rm.SwitchToState(rm.declare);
            }

            public override void Update(RoundManager rm)
            {
                // Select Dice if possible;
                rm.DiceManager.enabled = true;


            }
            public override void Exit(RoundManager rm)
            {
                //Disable listeners
                rm.DiceManager.enabled = false;
            }
        }
    }
}
