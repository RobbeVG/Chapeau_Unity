using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Seacore
{
    public class DeclareMenu : MonoBehaviour
    {
        [SerializeField]
        RoundStateMachine roundSM = null;
        [SerializeField]
        Dropdown[] dropdowns = new Dropdown[Globals.c_amountDie];

        public event Action OnEditDeclareRoll;

        private void Awake()
        {
            Assert.IsNotNull(roundSM, "Round State Machine Controller in the Declare Menu cannot be null");

            foreach (Dropdown item in dropdowns)
            {
                Assert.IsNotNull(item, "Not all dropdowns are filled in Declare Menu");
            }
        }

        public void RecalculateDeclaredRoll()
        {
            for (int i = 0; i < Globals.c_amountDie; i++)
            {
                roundSM.DeclaredRoll.ChangeValue(i, dropdowns[i].value);
            }
            roundSM.DeclaredRoll.CalculateResult();

            OnEditDeclareRoll?.Invoke();
        } 
    }
}
