using UnityEngine;

namespace Seacore.Common
{
    public interface IWindow
    {
        public string WindowName { get; }
        public bool Active { get; set; }

        public void OpenWindow(WindowManager windowManager);
        public void CloseWindow(WindowManager windowManager);
        public void ToggleWindow(WindowManager windowManager);
    }

    public class Window : MonoBehaviour, IWindow
    {
        public string WindowName { get { return gameObject.name; } }
        public bool Active { get { return gameObject.activeInHierarchy; } set { gameObject.SetActive(value); } }

        public void ToggleWindow(WindowManager windowManager)
        {
            if (Active)
                CloseWindow(windowManager);
            else
                OpenWindow(windowManager);
        }
        public void OpenWindow(WindowManager windowManager) => windowManager.OpenWindow(this);
        public void CloseWindow(WindowManager windowManager) => windowManager.CloseWindow(this);
    }
}