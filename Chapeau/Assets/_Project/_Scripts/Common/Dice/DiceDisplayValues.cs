using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

namespace Seacore.Common
{
    [CreateAssetMenu(fileName = "DiceDisplayValues", menuName = "ScriptableObjects/DiceDisplayValues")]
    public class DiceDisplayValues : ScriptableObject
    {
        [field: SerializeField]
        public SerializedDictionary<Die.Faces, Sprite> FaceSprites { get; private set; } = new SerializedDictionary<Die.Faces, Sprite>(5) { };
    }
}
