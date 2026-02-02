using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

using Seacore.Common;
using Seacore.Game;
using TMPro;

namespace Seacore.UI
{
    public class RollDisplay : MonoBehaviour
    {
        [SerializeField]
        private Roll roll;

        [SerializeField]
        private bool hideNone = true;

        [SerializeField]
        private DiceDisplayValues _diceDisplayValues = null;

        [SerializeField]
        private TMP_Text _textComponent;

        private Image _parentContentBackgground = null;

        private Image[] images = null;

        private void Awake()
        {
            Assert.IsNotNull(roll, "Roll display: roll is not assigned");
            Assert.IsNotNull(_diceDisplayValues, "Roll display: diceSprites is not assigned");
        }

        private void Start()
        {
            images = GetComponentsInChildren<Image>();
            Assert.IsTrue(images.Length == roll.Values.Length, $"Roll display: images found = { images.Length } and roll values = { roll.Values.Length }");

            _parentContentBackgground = GetComponentInParent<Image>();
        }

        private void Update()
        {
            _parentContentBackgground.enabled = roll.IsEmpty() ? false : true; 
            _textComponent.text = roll.IsEmpty() ? string.Empty : roll.ToLongString();
            
            for (int i = 0; i < roll.Values.Length; i++)
            {
                Image image = images[i];
                Die.Faces value = roll.Values[i];
                image.sprite = _diceDisplayValues.GetFaceSprite(value);
                if (hideNone && value == Die.Faces.None)
                    image.gameObject.SetActive(false);
                else
                    image.gameObject.SetActive(true);
            }
        }
    }
}
