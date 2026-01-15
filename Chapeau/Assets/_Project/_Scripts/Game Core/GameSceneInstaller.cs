using Reflex.Core;
using UnityEngine;

namespace Seacore.Game
{
    public class GameSceneInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            // Register game scene specific dependencies here
            builder.RegisterValue("Hello");
            builder.RegisterValue("GameScene");
            //builder.reg

        }
    }
}
