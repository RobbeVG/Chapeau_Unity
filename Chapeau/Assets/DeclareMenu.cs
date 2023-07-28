using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Seacore
{
    public class DeclareMenu : MonoBehaviour
    {
        [SerializeField]
        RoundStateMachineController roundSM = null;

        [SerializeField]
        Button confirmButton;

        [SerializeField]
        Dropdown[] dropdowns = new Dropdown[Globals.c_amountDie];

        private void Awake()
        {
            Assert.IsNotNull(roundSM, "Round State Machine Controller in the Declare Menu cannot be null");
            Assert.IsNotNull(confirmButton, "Confirm Button in the Declare Menu cannot be null");

            foreach (Dropdown item in dropdowns)
            {
                Assert.IsNotNull(item, "Not all dropdowns are filled in Declare Menu");
            }
        }

        private void OnEnable()
        {
            confirmButton.enabled = false;
        }

        public void OnConfirmButtonClick()
        {
            roundSM.ChangeRoundState(RoundStateType.PassOn);
        }

        public void RecalculateDeclaredRoll()
        {
            for (int i = 0; i < Globals.c_amountDie; i++)
            {
                roundSM.DeclaredRoll.ChangeValue(i, dropdowns[i].value);
            }
            roundSM.DeclaredRoll.CalculateResult();

            confirmButton.enabled = roundSM.DeclaredRoll > roundSM.CurrentRoll;
        } 
    }
}
