using UnityEngine;
using System;

namespace Seacore
{
    namespace RoundStates
    {
        [Serializable]
        public class Roll : RoundState
        {
            [SerializeField] GameEventCodedListener lookListener;
            [SerializeField] GameEventCodedListener declareListener;

            public override void Enter(RoundManager rm)
            {
                Log.Debug("Enter Roll");
                //Enable event listeners
                lookListener?.OnEnable(() => { 
                    rm.SwitchToState(rm.look);
                });

                declareListener?.OnEnable(() => { 
                    rm.SwitchToState(rm.declare);
                });

                //rm.PhysicalRoll.Sort();
                rm.DiceManager.Roll();
                rm.PhysicalRoll.CalculateResult();
                
                //Enable necesarry UI
                rm.UIManager.background.SetActive(true);
                rm.UIManager.lookButton.SetActive(true);
                rm.UIManager.declareButton.SetActive(true);

                Log.diceManager.Log(rm.PhysicalRoll.ToString());
            }

            public override void Update(RoundManager rm)
            {
                // Roll Dice;
                // Play tumbling dice sound
                // Await look or declare 
            }

            public override void Exit(RoundManager rm)
            {
                //Disabling event listeners
                lookListener.OnDisable();
                declareListener.OnDisable();

                //Disabling UI (look + declare)
                rm.UIManager.lookButton.SetActive(false);
                rm.UIManager.declareButton.SetActive(false);
            }
        }
    }
}
