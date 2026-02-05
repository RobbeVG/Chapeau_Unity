using Reflex.Core;
using Seacore.Common;
using Seacore.Common.Services;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore.Common
{
    public class RootInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField]
        [Tooltip("System Object Prefab")]
        GameObject _systems = null;

        [SerializeField]
        EGameState _startingGameState = EGameState.MainMenu;
        
        public void InstallBindings(ContainerBuilder builder)
        {
            Assert.IsNotNull(_systems, "Given System Object is null");
            GameObject systemInstance = Instantiate(_systems);
            DontDestroyOnLoad(systemInstance);

            Component[] components = systemInstance.GetComponentsInChildren<Component>(true);
            foreach (Component component in components)
            {
                if (component is Transform)
                    continue;

                builder.RegisterValue(component);
            }

            //Service Locators
            builder.RegisterValue(new QuitService());

            builder.RegisterValue(new GameState(_startingGameState));
        }
    }
}
