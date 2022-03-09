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
        Chapeau chapeau = null;

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
            Assert.IsTrue(images.Length == chapeau.roll.Values.Length, $"Roll display: images found = { images.Length } and roll values = { chapeau.roll.Values.Length }");
        }

        private void Update()
        {
            for (int i = 0; i < chapeau.roll.Values.Length; i++)
            {
                Die.DieValues value = chapeau.roll.Values[i];
                switch (value)
                {
                    case Die.DieValues.None:
                        images[i].sprite = none;
                        break;
                    case Die.DieValues.Nine:
                        images[i].sprite = nine;
                        break;
                    case Die.DieValues.Ten:
                        images[i].sprite = ten;
                        break;
                    case Die.DieValues.Jack:
                        images[i].sprite = jack;
                        break;
                    case Die.DieValues.Queen:
                        images[i].sprite = queen;
                        break;
                    case Die.DieValues.King:
                        images[i].sprite = king;
                        break;
                    case Die.DieValues.Ace:
                        images[i].sprite = ace;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
