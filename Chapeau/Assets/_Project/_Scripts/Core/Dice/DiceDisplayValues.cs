using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

namespace Seacore.Common
{
    [CreateAssetMenu(fileName = "DiceDisplayValues", menuName = "ScriptableObjects/DiceDisplayValues")]
    public class DiceDisplayValues : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<Die.Faces, Sprite> faceSprites = new SerializedDictionary<Die.Faces, Sprite>() 
        {
            { Die.Faces.None, null },
            { Die.Faces.Nine, null },
            { Die.Faces.Ten, null },
            { Die.Faces.Jack, null },
            { Die.Faces.Queen, null },
            { Die.Faces.King, null },
            { Die.Faces.Ace, null }
        };
    
        public Sprite GetFaceSprite(Die.Faces face) { return faceSprites[face]; }
    }
}
