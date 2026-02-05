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
            GameState gameState = Reflex.Core.Container.RootContainer.Resolve<GameState>();
            gameState.OnGameStateChange += SwitchUI;
            SwitchUI(gameState.Value);
        }

        private void SwitchUI(EGameState state)
        {
            switch (state)
            {
                case EGameState.MainMenu:
                    if (_inGameUI != null)
                        _inGameUI.SetActive(false);
                    if (_startMenuUI != null)
                        _startMenuUI.SetActive(true);
                    break;
                case EGameState.InGame:
                    if (_inGameUI != null)
                        _inGameUI.SetActive(true);
                    if (_startMenuUI != null)
                        _startMenuUI.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
}
