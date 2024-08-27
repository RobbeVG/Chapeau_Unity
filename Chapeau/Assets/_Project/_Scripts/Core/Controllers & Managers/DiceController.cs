using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

namespace Seacore
{
    /// <summary>
    /// Controls the behaviour of the dice.
    /// <para>Is responsible for hiding the dice, selecting the dice for rolling</para>
    /// </summary>
    [RequireComponent(typeof(DiceManager))]
    public class DiceController : MonoBehaviour
    {
        [Header("Settings for dice outline")]
        [SerializeField]
        private Color _toRollColor = Color.cyan;

        [SerializeField]
        private Color _toSelectOutline = Color.white;

        [SerializeField, MinMaxSlider(1.0f, 10.0f)]
        private Vector2 _OutlineOscillatingWidthValues = new Vector2(2.0f, 4.0f);

        [SerializeField, Range(1.0f, 20.0f), Tooltip("The frequency of the breathing effect per second")]
        private float _breathingFrequency = 1.0f;

        [SerializeField, Min(float.MinValue)]
        public float _durationRevealDice;

        [Header("References")]
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

        private Tweener _oscillationTween;


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
            _pickupDragController.ObjectPickedUp += OnDiePickUp;
            _pickupDragController.ObjectDropped += OnDieDrop;

            _objectSelector.OnHover += OnDieHover;
            _objectSelector.OnExit += OnDieExit;
            foreach (Die die in _diceManager.Dice) //Werkt niet want sommige dice moeten nog worden ingesteld
            {
                die.OnRolledValue += OnDieRolled;
            }
        }



        private void OnDisable()
        {
            _pickupDragController.ObjectPickedUp -= OnDiePickUp;
            _pickupDragController.ObjectDropped -= OnDieDrop;

            _objectSelector.OnHover -= OnDieHover;
            _objectSelector.OnExit -= OnDieExit;

            foreach (Die die in _diceManager.Dice)
            {
                die.OnRolledValue -= OnDieRolled;
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

            DOTween.Init();
        }

        private void OnDieHover(GameObject dieGameObject)
        {
            Die die = dieGameObject.GetComponent<Die>();

            Outline outline = _diceManager.DiceContainers[die].Outline;
            outline.enabled = true;

            _oscillationTween = DOVirtual.Float(_OutlineOscillatingWidthValues.x, _OutlineOscillatingWidthValues.y, 1.0f / _breathingFrequency, value =>
            {
                outline.OutlineWidth = value;
            }).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).Play();
        }

        private void OnDieExit(GameObject dieGameObject)
        {
            Die die = dieGameObject.GetComponent<Die>();
            DieInfo info = _diceManager.DiceContainers[die];

            if (!info.State.HasFlag(DieState.ToRoll))
            {
                Outline outline = _diceManager.DiceContainers[die].Outline;
                outline.enabled = false;
            }

            _oscillationTween.Kill(true);
            _oscillationTween = null;   
        }

        private void OnDieRolled(Die die)
        {
            die.Rigidbody.isKinematic = true;
            HideInsideDieImmediatly(_diceManager.DiceContainers[die]);
        }

        private void OnDiePickUp(GameObject objectDie)
        {
            Die die = objectDie.GetComponent<Die>();
            //die.Rigidbody.isKinematic = false;

            //Debug.Log("Die picked up");
        }

        private void OnDieDrop(GameObject objectDie)
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

        public void ToggleDieForRoll(Die die)
        {
            DieInfo dieInfo = _diceManager.DiceContainers[die];

            dieInfo.State ^= DieState.ToRoll; //Flip bit
            if (dieInfo.State.HasFlag(DieState.ToRoll))
            {
                dieInfo.Outline.OutlineColor = _toRollColor;
            }
            else
            {
                dieInfo.Outline.OutlineColor = _toSelectOutline;
            }
        }

        public void RevealDice()
        {
            foreach (Die die in _diceManager.Dice)
            {
                DieInfo dieInfo = _diceManager.DiceContainers[die];
                if (!dieInfo.State.HasFlag(DieState.Visible))
                {
                    DOVirtual.Float(1.0f, 0.0f, _durationRevealDice, value =>
                    {
                        UpdateMeshRendererPropertyBlock(dieInfo.MaterialPropertyBlock, dieInfo.MeshRenderer, value);

                    }).SetEase(Ease.InOutQuad);

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
