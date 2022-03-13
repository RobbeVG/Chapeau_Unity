using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//STATE ???
/// <summary>
/// 1. Roll
/// 2. Make Fictional Roll
/// 3. Do one of three (Chapeau!, Blind => Make Fictional Roll, Look => Back to roll)
/// 4. End -> Back to Roll
/// </summary>
namespace Seacore
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField]
        private Roll physicalRoll;
        [SerializeField]
        private GameObject tray;
        [SerializeField]    
        private Dictionary<Die, DieInfo> _dieContainers = new Dictionary<Die, DieInfo>();

        [SerializeField]
        private float rollDuration = 1.5f;
        [SerializeField]
        private float rollInterval = 0.1f;
        private float _elapsedSecondsRoll = 0.0f;
        private bool _rollHasFinished = false;


        public IEnumerator Roll()
        {
            Debug.Log("Started Rolling Dice");
            _elapsedSecondsRoll = 0.0f;
            while (_elapsedSecondsRoll < rollDuration)
            {
                _elapsedSecondsRoll += rollInterval;
                foreach (KeyValuePair<Die, DieInfo> dieInfoPair in _dieContainers)
                {
                    if (dieInfoPair.Value.IsPrimedToRoll)
                    {
                        dieInfoPair.Key.Throw(Random.insideUnitCircle * 1000, Random.insideUnitCircle * 100);
                    }
                }
                yield return new WaitForSeconds(rollInterval);
            }
            while (!_rollHasFinished)
            {
                yield return null;
            }
            Debug.Log("Finished Rolling Dice");
        }

        private void Awake() 
        {
            foreach (Die die in FindObjectsOfType<Die>())
            {
                _dieContainers[die] = new DieInfo { RolledTransform = null, IsInside = true, IsPrimedToRoll = true };
            }
        }
        private void OnEnable()
        {
            Die.OnSleep += RolledDie;
            Die.OnAwake += UnRollDie;
        }
        private void OnDisable()
        {
            Die.OnSleep -= RolledDie;
            Die.OnAwake -= UnRollDie;
        }
        private void RolledDie(Die die) 
        {
            _dieContainers[die].RolledTransform = die.transform;
            UpdatePhysicalRoll();
            CheckIfRollFinished();
        }
        private void UnRollDie(Die die)
        {
            _dieContainers[die].RolledTransform = null;
            UpdatePhysicalRoll();
        }

        private void CheckIfRollFinished()
        {
            bool allSleep = true;
            foreach (KeyValuePair<Die, DieInfo> item in _dieContainers)
            {
                Die die = item.Key;
                allSleep = die.Sleeping & allSleep;
            }
            _rollHasFinished = allSleep;
        }
        private void UpdatePhysicalRoll()
        {
            int counter = 0; 
            foreach (KeyValuePair<Die, DieInfo> item in _dieContainers)
            {
                Die die = item.Key;
                physicalRoll.ChangeValue(counter, die.RolledValue);
                counter++;
            }
        }


        private void Update()
        {
            //Lerp to other positions.
        }

        public IEnumerator SelectDice() 
        {
            yield return null;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
            {
                Die die = hitInfo.transform.GetComponent<Die>();
                if (die != null)
                {
                    Debug.Log("Die selected");
                    //rolledDiceTransforms[die] = ;
                }
            }
        }
    }
}
