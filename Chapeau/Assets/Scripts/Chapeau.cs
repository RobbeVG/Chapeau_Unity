using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class Chapeau : MonoBehaviour
    {
        public Roll roll = new Roll();

        private int _currentDieIndex = 0;

        private void OnEnable()
        {
            Die.OnRoll += OnDieRoll;
        }

        private void OnDisable()
        {
            Die.OnRoll -= OnDieRoll;
        }

        private void OnDieRoll(Die die)
        {
            roll.ChangeValue(_currentDieIndex++, die.RolledValue);
            
            if (_currentDieIndex == Roll.c_amountDie)
            {
                roll.Sort();
            }
        }
    }
}
