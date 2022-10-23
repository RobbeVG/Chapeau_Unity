using System;
using UnityEngine;

namespace Seacore
{
    namespace RoundStates
    {
        [Serializable]
        public class Action : RoundState
        {
            [SerializeField] GameEventCodedListener lookListener;
            [SerializeField] GameEventCodedListener declareListener;
            [SerializeField] GameEventCodedListener chapeauListener;

            public override void Enter(RoundManager rm)
            {
                lookListener?.OnEnable(() => {
                    rm.SwitchToState(rm.look);
                });
                declareListener?.OnEnable(() => {
                    rm.SwitchToState(rm.declare);
                });
                chapeauListener?.OnEnable(() => {
                    Debug.Log("Game ends");
                    //rm.SwitchToState(rm.look);
                });

                // Show options: Take action; Chapeau, Pass On;
                rm.UIManager.background.SetActive(true);
                rm.UIManager.lookButton.SetActive(true);
                rm.UIManager.declareButton.SetActive(true);
                rm.UIManager.chapeauButton.SetActive(true);

                }

            public override void Update(RoundManager rm)
            {
                // ?
            }
            
            public override void Exit(RoundManager rm)
            {
                //Disabling event listeners
                lookListener.OnDisable();
                declareListener.OnDisable();
                chapeauListener.OnDisable();

                //Disable menu
                rm.UIManager.lookButton.SetActive(false);
                rm.UIManager.declareButton.SetActive(false);
                rm.UIManager.chapeauButton.SetActive(false);
            }
        }
    }
}
