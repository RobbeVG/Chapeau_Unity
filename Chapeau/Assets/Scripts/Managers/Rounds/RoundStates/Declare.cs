using System;
using UnityEngine;
using UnityEngine.UI;

namespace Seacore
{
    namespace RoundStates
    {
        [Serializable]
        public class Declare : RoundState
        {
            [SerializeField] GameEventCodedListener confirmRollListener;
            private Button confirmingButton = null;

            public override void Enter(RoundManager rm)
            {
                confirmRollListener?.OnEnable(() => {
                    rm.SwitchToState(rm.action);
                });

                confirmingButton = rm.UIManager.confirmingRollButton.GetComponent<Button>();

                // Show delcare roll UI
                rm.UIManager.declaringRollDropdowns.SetActive(true);
                rm.UIManager.confirmingRollButton.SetActive(true);
            }

            public override void Update(RoundManager rm)
            {
                // Check if declared roll is higher than current roll -> Update UI Accordingly
                if (rm.DeclaredRoll <= rm.CurrentRoll)
                {
                    confirmingButton.interactable = false;
                }
                else 
                {
                    confirmingButton.interactable = true;
                }
            }
            public override void Exit(RoundManager rm)
            {
                //Disable listeners
                confirmRollListener?.OnDisable();

                //Sort given roll
                rm.DeclaredRoll.Sort();
                //Declared roll is now current roll
                rm.CurrentRoll.ChangeValueTo(rm.DeclaredRoll);
                
                //Disable UI
                rm.UIManager.declaringRollDropdowns.SetActive(false);
                rm.UIManager.confirmingRollButton.SetActive(false);

            }
        }
    }
}
