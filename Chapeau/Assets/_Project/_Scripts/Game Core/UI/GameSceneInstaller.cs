using Reflex.Core;
using UnityEngine;

namespace Seacore.Game
{
    public class GameSceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField]
        InputActionManager _inputActionManager = null;

        [SerializeField]
        GameRoundManager _roundManager = null;

        [SerializeField]
        UIGameController _UIGameController = null;


        public void InstallBindings(ContainerBuilder builder)
        {
            if (_inputActionManager != null)
            {
                builder.RegisterValue(_inputActionManager);
            }
            if (_roundManager != null)
            {
                builder.RegisterValue(_roundManager);
            }
            if (_UIGameController != null)
            {
                builder.RegisterValue(_UIGameController);
            }
        }
    }
}
