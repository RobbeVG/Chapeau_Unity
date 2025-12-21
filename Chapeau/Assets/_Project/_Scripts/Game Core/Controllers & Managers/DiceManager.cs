using Seacore.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Seacore.Game
{
    /// <summary>
    /// Manages dice objects, their state, and their interactions.
    /// </summary>
    public class DiceManager : MonoBehaviour
    {
        [Header("Instantiate settings")]        
        [SerializeField]
        [Tooltip("Must have a value between 0 and 31")]
        private int _spawnInLayer = 3;
        [SerializeField]
        private GameObject diePrefab;
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float paddingBetweenDice = 0.0f;

        private Dictionary<GameObject, Die> _gameObjectsToDie = new Dictionary<GameObject, Die>(Globals.c_amountDie);
        private Dictionary<Die, DieInfo> _diceContainers = new Dictionary<Die, DieInfo>(Globals.c_amountDie);

        public IReadOnlyDictionary<Die, DieInfo> DiceContainers => _diceContainers;
        public IReadOnlyCollection<Die> Dice => _diceContainers.Keys;

        [SerializeField]
        private int _totalDiceToInstantiate = Globals.c_amountDie; // Global constant for the total number of dice

        // Initialization
        private void Awake()
        {
            InitializeDice();
            // Automatically add children with Die components to the dictionary
            AddDiceFromChildren();
        }

        public void SetDieEventDelegates(Action<Die> hoverEnter, Action<Die> hoverExit, Action<Die> select, Action<Die> deselect)
        {
            foreach (var die in Dice)
            {
                GameObjectEventHandler dieEventHandler = die.GetComponent<GameObjectEventHandler>();
                if (dieEventHandler != null)
                {
                    dieEventHandler.OnHoverEnter = (GameObject gameObject) => hoverEnter(_gameObjectsToDie[gameObject]);
                    dieEventHandler.OnHoverExit = (GameObject gameObject) => hoverExit(_gameObjectsToDie[gameObject]);
                    dieEventHandler.OnSelected = (GameObject gameObject) => select(_gameObjectsToDie[gameObject]);
                    dieEventHandler.OnDeSelected = (GameObject gameObject) => deselect(_gameObjectsToDie[gameObject]);
                }
            }
        }   

        public Die GetDieFrom(GameObject gameObject)
        {
            if (_gameObjectsToDie.TryGetValue(gameObject, out Die die))
            {
                return die;
            }
            return null;
        }

        /// <summary>
        /// Splits the dice information into two groups based on the specified state.
        /// </summary>
        /// <param name="state">The <see cref="DieState"/> flag used to determine the grouping of dice.</param>
        /// <returns>A tuple containing two arrays of <see cref="DieInfo"/> objects: <list type="bullet"> <item> <description>The
        /// first array contains dice that match the specified state.</description> </item> <item> <description>The
        /// second array contains dice that do not match the specified state.</description> </item> </list> Both arrays
        /// are of fixed size, with unused elements set to <see langword="null"/>.</returns>
        public (Die[], Die[]) SplitDiceInfoBy(DieState state)
        {
            List<Die> validDice = new List<Die>(Globals.c_amountDie);
            List<Die> unvalidDice = new List<Die>(Globals.c_amountDie);

            foreach (var dieInfoPair in _diceContainers)
            {
                Die die = dieInfoPair.Key;
                DieInfo info = dieInfoPair.Value;
                if (info.State.HasFlag(state))
                {
                    validDice.Add(die);
                }
                else
                {
                    unvalidDice.Add(die);
                }
            }
            
            return (validDice.ToArray(), unvalidDice.ToArray());
        }

        private void InitializeDice()
        {
            if (diePrefab != null)
            {
                Vector3[] dieSpawnPoints = CalculateStartPositions();
                for (int i = 0; i < _totalDiceToInstantiate; i++)
                {
                    GameObject dieGameObject = Instantiate(diePrefab, dieSpawnPoints[i], Quaternion.identity, transform);
                    dieGameObject.name = diePrefab.name + "_" + i.ToString();
                    dieGameObject.layer = _spawnInLayer;

                    Outline outline;
                    if (!dieGameObject.TryGetComponent(out outline))
                        outline = dieGameObject.AddComponent<Outline>();
                    outline.enabled = false;
                    outline.OutlineMode = Outline.Mode.OutlineVisible;

                    Die die = dieGameObject.GetComponent<Die>();
                    
                    AddDie(die);
                }
            }
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
                _diceContainers.Add(die, new DieInfo(
                    index: _diceContainers.Count,
                    meshRenderer: die.GetComponent<MeshRenderer>(),
                    outline: die.GetComponent<Outline>(),
                    selectable: die.GetComponent<UnityEngine.UI.Selectable>()
                ));
            }
            _gameObjectsToDie[die.gameObject] = die;
        }

        private Vector3[] CalculateStartPositions()
        {
            if (diePrefab == null)
                return null;

            BoxCollider collider = diePrefab.GetComponent<BoxCollider>();
            if (collider == null)
                return null;

            if (_totalDiceToInstantiate <= 0)
                return null;

            Vector3[] dieSpawnPoints = new Vector3[_totalDiceToInstantiate];
            Vector3 spawnPosition = transform.position;
            spawnPosition.x -= (_totalDiceToInstantiate / 2) * (collider.size.x + paddingBetweenDice);
            spawnPosition.y += collider.size.y / 2.0f;

            for (int i = 0; i < _totalDiceToInstantiate; i++)
            {
                dieSpawnPoints[i] = spawnPosition;
                spawnPosition.x += collider.size.x + paddingBetweenDice;
            }

            return dieSpawnPoints;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Vector3[] dieSpawnPoints = CalculateStartPositions();
            if (dieSpawnPoints != null)
            {
                foreach (var point in dieSpawnPoints)
                {
                    Gizmos.DrawSphere(point, 0.1f);
                }
            }
        }

    }
    
    [Flags]
    [Serializable]
    public enum DieState : byte
    {
        None        = 0b_0000_0000, //0
        ToRoll      = 0b_0000_0001, //1
        Visible     = 0b_0000_0010, //2  -> NotVis when flag is off
        Selecting   = 0b_0000_0100, // 4
        Inside      = 0b_0001_0000, //16 -> Outside when flag is off
    }

    /// <summary>
    /// Holds additional information for each die. 
    /// Is class because it uses reference semantics
    /// </summary>
    [Serializable]
    public class DieInfo
    {
        private DieInfo() { }
        public DieInfo(int index, MeshRenderer meshRenderer, Outline outline, UnityEngine.UI.Selectable selectable)
        {
            Index = index;
            State = DieState.ToRoll | DieState.Inside;
            MeshRenderer = meshRenderer;
            MaterialPropertyBlock = new MaterialPropertyBlock();
            Outline = outline;
            Selectable = selectable;
        }

        public int Index { get; private set; }
        [field:SerializeField, ReadOnly]
        public DieState State { get; set; }
         // Add other properties as needed.
        public MeshRenderer MeshRenderer { get; private set; }
        public MaterialPropertyBlock MaterialPropertyBlock { get; private set; }
        public Outline Outline { get; private set; }
        public UnityEngine.UI.Selectable Selectable { get; private set; }
    }
}
