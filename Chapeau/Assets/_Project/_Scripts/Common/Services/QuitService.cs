using UnityEngine;

namespace Seacore.Common.Services
{
    public class QuitService : IService
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
