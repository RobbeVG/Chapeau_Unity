using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    [RequireComponent(typeof(DiceManager))]
    public class DiceController : MonoBehaviour
    {
        [SerializeField]
        private PickupDragController _pickupDragController;

        [SerializeField]
        private DiceManager _diceManager;

        private void Awake()
        {
            if (_pickupDragController == null)
                Debug.LogError("No pickup Controller attached");
        }

        private void OnEnable()
        {
            _pickupDragController.ObjectPickedUp += DiePickUp;
            _pickupDragController.ObjectDropped += DieDrop;
            foreach (Die die in _diceManager.Dice) //Werkt niet want sommige dice moeten nog worden ingesteld
            {
                die.OnRolledValue += OnDieRolled;
            }
        }

        private void OnDisable()
        {
            _pickupDragController.ObjectPickedUp -= DiePickUp;
            _pickupDragController.ObjectDropped -= DieDrop;
            foreach (Die die in _diceManager.Dice)
            {
                die.OnRolledValue -= OnDieRolled;
            }
        }

        private void OnDieRolled(Die die)
        {
            die.Rigidbody.isKinematic = true;
        }

        private void DiePickUp(GameObject objectDie)
        {
            Die die = objectDie.GetComponent<Die>();
            //die.Rigidbody.isKinematic = false;

            Debug.Log("Die picked up");
        }

        private void DieDrop(GameObject objectDie)
        {
            Die die = objectDie.GetComponent<Die>();
            //die.Rigidbody.isKinematic = true;

            Debug.Log("Die dropped");

        }
    }
}
