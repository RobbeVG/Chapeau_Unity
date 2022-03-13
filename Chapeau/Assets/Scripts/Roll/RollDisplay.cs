using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace Seacore
{
    public class RollDisplay : MonoBehaviour
    {
        [SerializeField]
        private Roll physicalRoll;

        [SerializeField]
        private Sprite none = null;        
        [SerializeField]
        private Sprite nine = null;
        [SerializeField]
        private Sprite ten = null;
        [SerializeField]
        private Sprite jack = null;
        [SerializeField]
        private Sprite queen = null;
        [SerializeField]
        private Sprite king = null;
        [SerializeField]
        private Sprite ace = null;

        private Image[] images = null;
        
        
        private void Start()
        {
            images = GetComponentsInChildren<Image>();
            Assert.IsTrue(images.Length == physicalRoll.Values.Length, $"Roll display: images found = { images.Length } and roll values = { physicalRoll.Values.Length }");
        }

        private void Update()
        {
            for (int i = 0; i < physicalRoll.Values.Length; i++)
            {
                Die.Faces value = physicalRoll.Values[i];
                switch (value)
                {
                    case Die.Faces.None:
                        images[i].sprite = none;
                        break;
                    case Die.Faces.Nine:
                        images[i].sprite = nine;
                        break;
                    case Die.Faces.Ten:
                        images[i].sprite = ten;
                        break;
                    case Die.Faces.Jack:
                        images[i].sprite = jack;
                        break;
                    case Die.Faces.Queen:
                        images[i].sprite = queen;
                        break;
                    case Die.Faces.King:
                        images[i].sprite = king;
                        break;
                    case Die.Faces.Ace:
                        images[i].sprite = ace;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
