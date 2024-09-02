using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Seacore
{
    public class DeclareMenu : MonoBehaviour
    {
        [SerializeField]
        Dropdown[] dropdowns = new Dropdown[Globals.c_amountDie];

        Roll _declaredRoll = null;

        public event Action OnEditDeclareRoll;

        private void Awake()
        {
            _declaredRoll = Resources.Load<Roll>("Rolls/DeclaredRoll");
            Assert.IsNotNull(_declaredRoll, "Declared Roll in the Declare Menu cannot be null");

            foreach (Dropdown item in dropdowns)
            {
                Assert.IsNotNull(item, "Not all dropdowns are filled in Declare Menu");
            }
        }

        public void RecalculateDeclaredRoll()
        {
            for (int i = 0; i < Globals.c_amountDie; i++)
            {
                _declaredRoll.ChangeValue(i, dropdowns[i].value);
            }
            _declaredRoll.CalculateResult();

            OnEditDeclareRoll?.Invoke();
        } 
    }
}
