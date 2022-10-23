using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace Seacore
{
    public class RollDisplay : MonoBehaviour
    {
        [SerializeField]
        private Roll roll;

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
            Assert.IsTrue(images.Length == roll.Values.Length, $"Roll display: images found = { images.Length } and roll values = { roll.Values.Length }");
        }

        private void Update()
        {
            for (int i = 0; i < roll.Values.Length; i++)
            {
                Image image = images[i];
                image.gameObject.SetActive(true);
                Die.Faces value = roll.Values[i];
                switch (value)
                {
                    case Die.Faces.None:
                        image.gameObject.SetActive(false);
                        image.sprite = none;
                        break;
                    case Die.Faces.Nine:
                        image.sprite = nine;
                        break;
                    case Die.Faces.Ten:
                        image.sprite = ten;
                        break;
                    case Die.Faces.Jack:
                        image.sprite = jack;
                        break;
                    case Die.Faces.Queen:
                        image.sprite = queen;
                        break;
                    case Die.Faces.King:
                        image.sprite = king;
                        break;
                    case Die.Faces.Ace:
                        image.sprite = ace;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
