using DG.Tweening;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSelectorConfig", menuName = "ScriptableObjects/Dice Selector Config", order = 1)]
public class DiceSelectorConfig : ScriptableObject
{
    [field: SerializeField]
    public Ease EaseType { get; private set; } = Ease.InOutSine;
    [field: SerializeField]
    public int NudgeAmount { get; private set; } = 5;
    [field: SerializeField]
    public float NudgeDuration { get; private set; } = 0.1f;
    [field: SerializeField]
    public float FadeDuration { get; private set; } = 0.3f;
    [field: SerializeField]
    public float EndYOffset { get; private set; } = 75f;
}
