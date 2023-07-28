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
        #region Fields
        #region Inspector Variables
        [SerializeField]
        private Roll physicalRoll;
        [SerializeField]
        private GameObject diePrefab;
        [SerializeField][Range(0.0f, 10.0f)]
        private float paddingBetweenDice = 0.0f;
        #endregion

        #region Private variables
        private Dictionary<Die, DieInfo> _dieContainers = new Dictionary<Die, DieInfo>(Globals.c_amountDie);
        private Vector3[] _dieSpawnPoints = new Vector3[Globals.c_amountDie];
        #endregion

        #endregion Fields
        //Initialization
        private void OnValidate()
        {
            CalculateStartPositions();
        }
        private void Awake()
        {
            CalculateStartPositions();
            for (int i = 0; i < Globals.c_amountDie; i++)
            {
                //Spawning die -> and create dieInfo
                GameObject dieGameObject = Instantiate(diePrefab, _dieSpawnPoints[i], Quaternion.identity, transform);
                dieGameObject.name = "Die_" + i.ToString();
                Die die = dieGameObject.GetComponent<Die>();
                Outline outline = die.GetComponent<Outline>();

                
                //Material mat = die.GetComponent<Renderer>().sharedMaterial;
                _dieContainers[die] = new DieInfo { Index = i, Outline = outline };
            }
        }
        private void CalculateStartPositions()
        {
            if (diePrefab == null)
                return;
            BoxCollider collider = diePrefab.GetComponent<BoxCollider>();
            if (collider == null)
                return;

            Vector3 spawnPosition = transform.position;
            spawnPosition.x -= (Globals.c_amountDie / 2) * (collider.size.x + paddingBetweenDice);
            spawnPosition.y += collider.size.y / 2.0f;

            for (int i = 0; i < Globals.c_amountDie; i++)
            {
                _dieSpawnPoints[i] = spawnPosition;
                spawnPosition.x += collider.size.x + paddingBetweenDice;
            }
        }

        //Gizmo
        private void OnDrawGizmos()
        {
            if (diePrefab == null)
                return;
            BoxCollider collider = diePrefab.GetComponent<BoxCollider>();
            if (collider == null)
                return;

            for (int i = 0; i < Globals.c_amountDie; i++)
            {
                Gizmos.matrix = Matrix4x4.TRS(_dieSpawnPoints[i], Quaternion.identity, Vector3.one);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(Vector3.zero, 0.25f);
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(collider.center, collider.size);
            }
        }

        //TODO Roll inside or outside
        public void Roll()
        {
            foreach (KeyValuePair<Die, DieInfo> dieInfoPair in _dieContainers)
            {
                Die die = dieInfoPair.Key;
                DieInfo info = dieInfoPair.Value;

                die.SetRolledValue((Die.Faces)UnityEngine.Random.Range((int)Die.Faces.Nine, (int)Die.Faces.Ace) + 1);
                physicalRoll.ChangeValue(info.Index, die.DieValue);
            }
        }
    }
}
/*
// Inspector variables
 *         [SerializeField]
        private float rollDuration = 1.5f;
        [SerializeField]
        private float rollInterval = 0.1f;
        [SerializeField]
        private AnimationCurve translateDiceCurve = new AnimationCurve();
    [Header("WIP")]
    [SerializeField]
    private Color disableColor;
    [SerializeField]
    private Color outsideColor;
    [SerializeField]
    private Color bothColor;

//Private variables
    private Die _currentlyHoveredDie = null;
    private State _currentState = State.Finished;
    private float _elapsedSeconds = 0.0f;

    #region Events / Delegates
    //Gets subscirbed and unsubscribed from OnEnable/OnDisable

    private void RolledDie(Die die) 
    {
        _dieContainers[die].RolledPosition = die.transform.position;
        physicalRoll.ChangeValue(_dieContainers[die].Index, die.RolledValue);
        if (IsRollFinished())
            _currentState = State.StoppedRolling; //Need for in between state of rolling -> translating as we use State.Translating as a condition. Otherwise die keep waking up.

        bool IsRollFinished()
        {
            bool allSleep = true;
            ForEachDie((die) => { allSleep &= die.Rolled; });
            return allSleep;
        }
    }
    private void UnRolledDie(Die die)
    {
        physicalRoll.ChangeValue(_dieContainers[die].Index, die.RolledValue);
    }
    #endregion

    #region LifeCycle Methods
    //Initialization
    private void Awake()
    {
        for (int i = 0; i < c_amountDice; i++)
        {
            //Spawning die -> and create dieInfo
            GameObject dieGameObject = Instantiate(diePrefab, _spawnPoints[i], Quaternion.identity, transform);
            dieGameObject.name = "Die_" + i.ToString();
            Die die = dieGameObject.GetComponent<Die>();
            Outline outline = die.GetComponent<Outline>();
            Material mat = die.GetComponent<Renderer>().material;
            _dieContainers[die] = new DieInfo { Index = i, DieMateriel = mat, Outline = outline };
        }
    }
    private void OnEnable()
    {
        Die.OnRoll += RolledDie;
        Die.OnUnroll += UnRolledDie;
    }
    //Editor
    private void OnValidate()
    {
        if (diePrefab == null)
            return;
        BoxCollider collider = diePrefab.GetComponent<BoxCollider>();
        if (collider == null)
            return;

        Vector3 spawnPosition = transform.position;
        spawnPosition.x -= (c_amountDice / 2) * (collider.size.x + paddingBetweenDice);
        spawnPosition.y += collider.size.y / 2.0f;

        for (int i = 0; i < c_amountDice; i++)
        {
            _spawnPoints[i] = spawnPosition;
            spawnPosition.x += collider.size.x + paddingBetweenDice;
        }
    }
    //Physics
    private void FixedUpdate()
    {
        if (_currentState == State.Translating)
        {
            float curveEndTime = translateDiceCurve.keys.Last().time;
            if (_elapsedSeconds >= curveEndTime)
            {
                ForEachDieInfo((die, info) => 
                {
                    Vector3 targetRotation = die.transform.rotation.eulerAngles;
                    targetRotation.y = 0.0f;

                    die.transform.position = _spawnPoints[info.Index];
                    die.transform.rotation = Quaternion.Euler(targetRotation);
                });

                _currentState = State.Finished;
            }
            else
            {
                ForEachDieInfo((die, info) =>
                {
                    Vector3 targetPosition = _spawnPoints[info.Index];
                    Vector3 targetRotation = die.transform.rotation.eulerAngles;
                    targetRotation.y = 0.0f;

                    die.transform.position = Vector3.Lerp(_dieContainers[die].RolledPosition, targetPosition, translateDiceCurve.Evaluate(_elapsedSeconds));
                    die.transform.rotation = Quaternion.Slerp(die.transform.rotation, Quaternion.Euler(targetRotation), translateDiceCurve.Evaluate(_elapsedSeconds));
                });

                _elapsedSeconds += Time.fixedDeltaTime;
            }
        }
    }

    //Decommissioning
    private void OnDisable()
    {
        Die.OnRoll -= RolledDie;
        Die.OnUnroll -= UnRolledDie;
    }
    #endregion

    public void TrySelectDice() 
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
        {
            Die die = hitInfo.transform.GetComponent<Die>();
            if (die != null)
            {
                if (_currentlyHoveredDie != null)
                    if (_currentlyHoveredDie != die)
                        _dieContainers[_currentlyHoveredDie].Outline.enabled = false;
                _currentlyHoveredDie = die;
                _dieContainers[die].Outline.enabled = true;
                if (Input.GetMouseButtonDown(0))
                {
                    if (_dieContainers[die].CurrentState.HasFlag(DieInfo.State.Disable | DieInfo.State.Outside))
                        _dieContainers[die].CurrentState = DieInfo.State.None;
                    else
                        _dieContainers[die].CurrentState += 1;

                    switch (_dieContainers[die].CurrentState)
                    {
                        case DieInfo.State.None:
                            _dieContainers[die].DieMateriel.color = Color.white;
                            break;
                        case DieInfo.State.Outside:
                            _dieContainers[die].DieMateriel.color = outsideColor;
                            break;
                        case DieInfo.State.Disable:
                            _dieContainers[die].DieMateriel.color = disableColor;
                            break;
                        case DieInfo.State.Outside | DieInfo.State.Disable:
                            _dieContainers[die].DieMateriel.color = bothColor;
                            break;
                    }
                    Debug.Log("Die selected" + _dieContainers[die].CurrentState);
                        
                }
                return;
            }
        }
        if (_currentlyHoveredDie != null)
            _dieContainers[_currentlyHoveredDie].Outline.enabled = false;
        _currentlyHoveredDie = null;
    }
    public IEnumerator Roll()
    {
        ForEachDie((die) => { die.Kinematic = false; });
        //Rolling
        _currentState = State.Rolling;
        _elapsedSeconds = 0.0f;
        while (_elapsedSeconds < rollDuration)
        {
            ForEachDieInfo((die, info) =>
            {
                if (!info.CurrentState.HasFlag(DieInfo.State.Disable))
                {
                    die.Throw(UnityEngine.Random.insideUnitCircle * 500, UnityEngine.Random.insideUnitCircle * 100);
                }
            }); 
            yield return new WaitForSeconds(rollInterval);
            _elapsedSeconds += rollInterval;
        }
        while (_currentState == State.Rolling)
            yield return null;

        //Translation
        _currentState = State.Translating;
        _elapsedSeconds = 0.0f;
        ForEachDie((die) => { die.Kinematic = true; });
        while (_currentState == State.Translating) //Proceeded in FixedUpdate because we are dealing with movement!
            yield return null;
        //Translation wakes up the dice, even when kinematic
        ForEachDie((die) => { die.TriggerSleep(); }); //Makes sure die does not move over upcoming frames
    }

    #region Helper functions
    private void ForEachDie(Action<Die> lambda)
    {
        foreach (KeyValuePair<Die, DieInfo> dieInfoPair in _dieContainers)
        {
            lambda(dieInfoPair.Key);
        }
    }
    private void ForEachDieInfo(Action<Die, DieInfo> lambda)
    {
        foreach (KeyValuePair<Die, DieInfo> dieInfoPair in _dieContainers)
        {
            lambda(dieInfoPair.Key, dieInfoPair.Value);
        }
    }
    #endregion

    //Nested types
    enum State : byte { Rolling, StoppedRolling, Translating, Finished }
*/
