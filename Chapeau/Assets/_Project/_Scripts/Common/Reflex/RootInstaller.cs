using Reflex.Core;
using UnityEngine;
using Seacore.Common.Services;
using UnityEngine.Assertions;

namespace Seacore.Common
{
    public class RootInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField]
        [Tooltip("System Object Prefab")]
        GameObject _systems = null;
        
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
            //builder.RegisterType(typeof(AudioManager), Reflex.Enums.Lifetime.Singleton, Reflex.Enums.Resolution.Eager);
        }
    }
}
