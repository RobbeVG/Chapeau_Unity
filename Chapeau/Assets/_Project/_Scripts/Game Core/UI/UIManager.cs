using UnityEngine;
using Seacore.Common;
using Reflex.Attributes;

namespace Seacore.Game
{
    public class UIManager : MonoBehaviour
    {
        [Inject]
        private InputActionManager _inputActionManager = null;
        [SerializeField]
        private WindowManager _windowManager = null;

        [SerializeField]
        GameObject _inGameUI = null;

        [SerializeField]
        GameObject _startMenuUI = null;


        // Start is called before the first frame update
        void Start()
        {
            _inputActionManager.OnWindowCancel += _windowManager.OnCancel;
            if (_inGameUI != null)
                _inGameUI.SetActive(false);
            if (_startMenuUI != null)
                _startMenuUI.SetActive(true);
        }
    }
}
