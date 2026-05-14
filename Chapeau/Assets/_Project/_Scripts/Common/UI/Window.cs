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
        /// <summary>
        /// Gets or sets a value indicating whether the GameObject is active in the scene hierarchy.
        /// </summary>
        /// <remarks>Setting this property enables or disables the GameObject and all its children in the
        /// hierarchy. When set to false, the GameObject and its children will not receive update calls or be rendered.
        /// Changing this property at runtime can affect component behavior and event execution.</remarks>
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