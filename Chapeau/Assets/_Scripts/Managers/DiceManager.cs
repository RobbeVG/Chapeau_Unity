using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public enum RollLocation : byte
    {
        Inside, Outside
    }

    /// <summary>
    /// Manages dice objects, their state, and their interactions.
    /// </summary>
    public class DiceManager : MonoBehaviour
    {
        [SerializeField]
        private Roll physicalRoll;
        [SerializeField]
        private GameObject diePrefab;
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float paddingBetweenDice = 0.0f;

        private RollLocation _rollLocation = RollLocation.Inside;
        private Dictionary<Die, DieInfo> _diceContainers = new Dictionary<Die, DieInfo>(Globals.c_amountDie);

        public IReadOnlyDictionary<Die, DieInfo> DiceContainers => _diceContainers;
        public IReadOnlyCollection<Die> Dice => _diceContainers.Keys;

        [SerializeField]
        private int _totalDiceToInstantiate = Globals.c_amountDie; // Global constant for the total number of dice

        private void Start()
        {
            // Automatically add children with Die components to the dictionary
            AddDiceFromChildren();
        }

        private void AddDiceFromChildren()
        {
            Die[] childDice = GetComponentsInChildren<Die>();

            foreach (var die in childDice)
            {
                AddDie(die);
            }
        }

        private void AddDie(Die die)
        {
            if (!_diceContainers.ContainsKey(die))
            {
                _diceContainers.Add(die, new DieInfo { Index = _diceContainers.Count });
            }
        }

        private void InitializeDice()
        {
            if (diePrefab != null)
            {
                Vector3[] dieSpawnPoints = CalculateStartPositions();
                for (int i = 0; i < _totalDiceToInstantiate; i++)
                {
                    GameObject dieGameObject = Instantiate(diePrefab, dieSpawnPoints[i], Quaternion.identity, transform);
                    dieGameObject.name = "Die_" + i.ToString();

                    Die die = dieGameObject.GetComponent<Die>();
                    _diceContainers[die] = new DieInfo { Index = i };
                }
            }
        }

        // Initialization
        private void Awake()
        {
            InitializeDice();
        }

        /// <summary>
        /// Rolls the dice that are located at the current roll location.
        /// </summary>
        public void Roll()
        {
            foreach (var pair in _diceContainers)
            {
                Die die = pair.Key;
                DieInfo info = pair.Value;

                if (info.Location == _rollLocation)
                {
                    die.Roll();
                    physicalRoll.ChangeValue(info.Index, die.DieValue);
                }
            }
        }

        private Vector3[] CalculateStartPositions()
        {
            if (diePrefab == null)
                return null;

            BoxCollider collider = diePrefab.GetComponent<BoxCollider>();
            if (collider == null)
                return null;

            Vector3[] dieSpawnPoints = new Vector3[Globals.c_amountDie];
            Vector3 spawnPosition = transform.position;
            spawnPosition.x -= (Globals.c_amountDie / 2) * (collider.size.x + paddingBetweenDice);
            spawnPosition.y += collider.size.y / 2.0f;

            for (int i = 0; i < Globals.c_amountDie; i++)
            {
                dieSpawnPoints[i] = spawnPosition;
                spawnPosition.x += collider.size.x + paddingBetweenDice;
            }

            return dieSpawnPoints;
        }

        // Event handling for dice rolls and unrolls could be added here if needed.

        // Additional methods for managing dice states, such as selection, highlighting, etc., could be included here.
    }

    /// <summary>
    /// Holds additional information for each die.
    /// </summary>
    public class DieInfo
    {
        public int Index { get; set; }
        public RollLocation Location { get; set; } = RollLocation.Inside;
        // Add other properties as needed.
    }
}
