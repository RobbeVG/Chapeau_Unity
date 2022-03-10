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
                roll.CalculateResult();

                Debug.Log(roll.Result.ToString());
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                roll.Clear();
                _currentDieIndex = 0;
                foreach (var item in GetComponentsInChildren<Die>())
                {
                    item.Throw(Random.insideUnitSphere * 1000, Random.insideUnitSphere * 50);
                }    
            }
        }
    }
}
