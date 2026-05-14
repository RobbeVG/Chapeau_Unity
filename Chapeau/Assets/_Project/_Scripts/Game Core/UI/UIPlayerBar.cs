using Seacore.Game;
using Seacore.Logger;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Seacore.UI
{
    public class UIPlayerBar : MonoBehaviour
    {
        Button button = null;
        TMP_InputField inputField = null;

        public Button ButtonComponent { get { return button; } }
        public string PlayerName { get { return inputField.text; } set { inputField.text = value; } }

        private void Awake()
        {
            button = GetComponentInChildren<Button>(true);
            inputField = GetComponentInChildren<TMP_InputField>(true);

            if (button == null)
            {
                SCLogger.Log("Button not found");
            }
            if (inputField == null)
            {
                SCLogger.Log("InputField not found");
            }
        }

        public void AddListenerButton(Action<UIPlayerBar> action)
        {
            button.onClick.AddListener(() => action.Invoke(this));
        }
    }
}
