using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Seacore.Game
{
    /// <summary>
    /// Handles the visual effects and animations for dice, including color changes, outline effects, and reveal animations.
    /// </summary>
    /// <remarks>This class is responsible for managing the visual representation of dice, such as applying colors for
    /// different states (e.g., rolling, selection), animating outline effects, and controlling the duration of dice reveal
    /// animations. It provides configurable properties for customizing these effects.</remarks>
    [RequireComponent(typeof(DiceManager))]
    
    public class DieVisualHandler : MonoBehaviour
    {
        [SerializeField] DiceVisualsConfig _dieVisuals = null;
        [SerializeField, Min(float.MinValue)] float _durationRevealDice = 1.0f;

        //No instance has to be created of the dice's material
        readonly int _fadeMaterialDiceTransitionID = Shader.PropertyToID("_Transition");
        DiceManager _diceManager;
        Tweener _oscillationTween = null;
        float _outlineWidthValue = 0.0f;

        public void Awake()
        {
            Assert.IsNotNull(_dieVisuals, "DieVisualsConfig is not assigned in DieVisualHandler. Please assign it in the inspector");
            _diceManager = GetComponent<DiceManager>();
        }

        public void OnEnable()
        {
            _oscillationTween = DOTween.To(value => _outlineWidthValue = value, _dieVisuals.OutlineOscillatingWidthValues.x, _dieVisuals.OutlineOscillatingWidthValues.y, _dieVisuals.BreathingFrequency)
                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).Play();
        }

        public void OnDisable()
        {
            if (_oscillationTween != null && _oscillationTween.IsActive())
            {
                _oscillationTween.Kill();
                _oscillationTween = null;
            }
        }

        private void Update()
        {
            foreach (var dieInfoPair in _diceManager.DiceContainers)
            {
                //Die die = dieInfoPair.Key;
                DieInfo info = dieInfoPair.Value;

                info.Outline.OutlineWidth = _outlineWidthValue;
                info.Outline.OutlineColor = info.State.HasFlag(DieState.Selecting)? _dieVisuals.ToSelectOutline : _dieVisuals.ToRollCollor;
                info.Outline.enabled = Convert.ToBoolean(info.State & (DieState.ToRoll | DieState.Selecting));
            }
        }

        public void RevealDie(DieInfo dieInfo)
        {
            DOVirtual.Float(1.0f, 0.0f, _durationRevealDice, value =>
            {
                UpdateMeshRendererPropertyBlock(dieInfo, value);

            }).SetEase(Ease.InOutQuad);
        }

        public void HideDieImmediatly(DieInfo dieInfo)
        {
            UpdateMeshRendererPropertyBlock(dieInfo, 1.0f);
        }

        private void UpdateMeshRendererPropertyBlock(DieInfo dieInfo, float value)
        {
            MaterialPropertyBlock currentBlock = dieInfo.MaterialPropertyBlock;
            MeshRenderer meshRenderer = dieInfo.MeshRenderer;
            currentBlock.SetFloat(_fadeMaterialDiceTransitionID, value);
            meshRenderer.SetPropertyBlock(currentBlock);
        }
    }
}

