using DG.Tweening;
using Seacore.Common;
using UnityEngine;


namespace Seacore.Game
{
    [CreateAssetMenu(fileName = "DiceConfiguration", menuName = "ScriptableObjects/DiceConfig")]
    public class DiceConfig : ScriptableObject
    {

    }

    [CreateAssetMenu(fileName = "DiceVisualizationConfiguration", menuName = "ScriptableObjects/DiceVisualConfig")]

    public class DiceVisualsConfig : ScriptableObject
    {
        /// <summary>
        /// The color used to indicate a dice is ready to be rolled.
        /// </summary>
        [field: SerializeField]
        public Color ToRollCollor { get; private set; } = new Color(0.9056604f, 0.6993408f, 0.2776789f);

        /// <summary>
        /// The color used for the outline when a dice is selected.
        /// </summary>
        [field: SerializeField]
        public Color ToSelectOutline { get; private set; } = Color.white;

        /// <summary>
        /// The minimum and maximum width values for the oscillating outline effect.
        /// </summary>
        [field: SerializeField, MinMaxSlider(1.0f, 10.0f)]
        public Vector2 OutlineOscillatingWidthValues { get; private set; } = new Vector2(3.0f, 5.0f);

        /// <summary>
        /// The frequency of the breathing (oscillation) effect for the dice outline.
        /// </summary>
        [field: SerializeField, Range(1.0f, 20.0f)]
        [Tooltip("The frequency of the breathing effect per second")]
        public float BreathingFrequency { get; private set; } = 1.0f;

        /// <summary>
        /// Gets the duration, in seconds, for revealing the dice.
        /// </summary>
        [field: SerializeField, Min(float.MinValue)]
        public float DurationRevealDice { get; private set; } = 0.0f;

        /// <summary>
        /// Gets the tweener responsible for handling oscillation animations.
        /// </summary>
        public Tweener OscillationTween { get; private set; }
    }
}