using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



namespace Seacore
{
    [Serializable]
    public class UIButtonManager<EButtonType> where EButtonType : Enum
    {
        [SerializedDictionary("Button Type", "Linked Button")]
        [SerializeField]
        private SerializedDictionary<EButtonType, Button> _buttons = new SerializedDictionary<EButtonType, Button>(
            Enum.GetValues(typeof(EButtonType)).Cast<EButtonType>().Select(e => new KeyValuePair<EButtonType, Button>(e, null)).ToArray());

        public Button this[EButtonType key]
        {
            get => _buttons[key];
        }

        public void HideAllButtons()
        {
            foreach (var button in _buttons.Values)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}