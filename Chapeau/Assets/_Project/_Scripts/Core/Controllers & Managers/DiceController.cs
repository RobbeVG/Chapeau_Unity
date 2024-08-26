using System.Collections;
using UnityEngine;

namespace Seacore
{
    /// <summary>
    /// Controls the behaviour of the dice.
    /// <para>Is responsible for hiding the dice, selecting the dice for rolling</para>
    /// </summary>
    [RequireComponent(typeof(DiceManager))]
    public class DiceController : MonoBehaviour
    {
        [SerializeField]
        private ObjectSelector _objectSelector;

        [SerializeField]
        private PickupAndDrag _pickupDragController;

        [SerializeField]
        private CircleController _circleController;

        [SerializeField]
        private Material _fadeMaterialDice;

        //No instance has to be created of the dice's material
        readonly int _fadeMaterialDiceTransitionID = Shader.PropertyToID("_Transition"); 

        private DiceManager _diceManager;

        private void Awake()
        {
            if (_pickupDragController == null)
                Debug.LogError("No pickup Controller attached");
            if (_circleController == null)
                Debug.LogError("No circle Controller attached");

            _diceManager = GetComponent<DiceManager>();
        }

        private void OnEnable()
        {
            _pickupDragController.ObjectPickedUp += DiePickUp;
            _pickupDragController.ObjectDropped += DieDrop;
            _objectSelector.OnHover += OnDieHover;
            foreach (Die die in _diceManager.Dice) //Werkt niet want sommige dice moeten nog worden ingesteld
            {
                die.OnRolledValue += OnDieRolled;
            }
        }

        private void Start()
        {
            foreach (Die die in _diceManager.Dice)
            {
                DieInfo dieInfo = _diceManager.DiceContainers[die];
                dieInfo.MeshRenderer.material = _fadeMaterialDice;
                HideInsideDieImmediatly(dieInfo);
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

        private void OnDieHover(GameObject dieGameObject)
        {
            Die die = dieGameObject.GetComponent<Die>();
            _diceManager.DiceContainers[die].Outline.enabled = false;
        }

        private void OnDieRolled(Die die)
        {
            die.Rigidbody.isKinematic = true;
            HideInsideDieImmediatly(_diceManager.DiceContainers[die]);
        }

        private void DiePickUp(GameObject objectDie)
        {
            Die die = objectDie.GetComponent<Die>();
            //die.Rigidbody.isKinematic = false;

            //Debug.Log("Die picked up");
        }

        private void DieDrop(GameObject objectDie)
        {
            Die die = objectDie.GetComponent<Die>();
            //die.Rigidbody.isKinematic = true;

            if (_circleController.IsPositionInCircle(objectDie.transform.position))
            {
                _diceManager.DiceContainers[die].State |= DieState.Inside;
            }
            else
            {
                _diceManager.DiceContainers[die].State &= ~DieState.Inside;
            }
        }

        public void SelectDieForRoll(Die die)
        {

            Debug.Log("Implement Select Die for Roll");
        }

        public void RevealDice()
        {
            foreach (Die die in _diceManager.Dice)
            {
                DieInfo dieInfo = _diceManager.DiceContainers[die];
                if (!dieInfo.State.HasFlag(DieState.Visible))
                {
                    StartCoroutine(TransitionMaterialDieTo(dieInfo));
                    dieInfo.State |= DieState.Visible;
                }
            }
        }

        public void HideInsideDiceImmediatly()
        {
            foreach (Die die in _diceManager.Dice)
            {
                HideInsideDieImmediatly(_diceManager.DiceContainers[die]);
            }
        }
        private void HideInsideDieImmediatly(DieInfo dieInfo)
        {
            if (dieInfo.State.HasFlag(DieState.Inside))
            {
                UpdateMeshRendererPropertyBlock(dieInfo, 1.0f);
                dieInfo.State &= ~DieState.Visible;
            }
        }

        private IEnumerator TransitionMaterialDieTo(DieInfo dieInfo, bool ToVisible = true, float duration = 2.0f)
        {
            float timeElapsed = 0f;

            MaterialPropertyBlock currentBlock = dieInfo.MaterialPropertyBlock;
            MeshRenderer meshRenderer = dieInfo.MeshRenderer;

            while (timeElapsed < duration)
            {
                float t = timeElapsed / duration;
                float newvalue = ToVisible ? 1.0f - t : t;
                UpdateMeshRendererPropertyBlock(currentBlock, meshRenderer, EasingFunction.EaseInOutQuad(0, 1.0f, newvalue));

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            UpdateMeshRendererPropertyBlock(currentBlock, meshRenderer, ToVisible ? 0.0f : 1.0f);
        }

        private void UpdateMeshRendererPropertyBlock(DieInfo dieInfo, float value) 
        {
            MaterialPropertyBlock currentBlock = dieInfo.MaterialPropertyBlock;
            MeshRenderer meshRenderer = dieInfo.MeshRenderer;
            UpdateMeshRendererPropertyBlock(currentBlock, meshRenderer, value);
        }
        private void UpdateMeshRendererPropertyBlock(MaterialPropertyBlock materialPropertyBlock, MeshRenderer meshRenderer, float value)
        {
            materialPropertyBlock.SetFloat(_fadeMaterialDiceTransitionID, value);
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}
