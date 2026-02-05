using UnityEngine;

namespace Seacore.Common.Services
{
    public interface IQuitService : IService
    {
        void QuitApplication();
    }

    public class QuitService : IQuitService
    {
        public void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
