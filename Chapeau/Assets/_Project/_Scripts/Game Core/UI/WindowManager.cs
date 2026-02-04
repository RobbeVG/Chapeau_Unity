using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore.Game
{
    public class WindowManager : MonoBehaviour
    {
        [Inject]
        InputActionManager _inputActionManager = null; //On cancel event

        [SerializeField]
        [Tooltip("If no window is present, open this on cancel event - [escape]")]
        private Window _defaultWindow = null;
        
        private IWindow[] _windows = null;
        private Stack<IWindow> _activeWindows = new Stack<IWindow>();


        private void Awake()
        {
            _inputActionManager.OnWindowCancel += OnCancel;
            _windows = GetComponentsInChildren<IWindow>(true);
            if (_windows.Length == 0)
                Debug.LogError("No windows found on Window Manager", this);
        }

        private void Start()
        {
            foreach (IWindow window in _windows)
            {
                if (window.Active)
                {
                    _activeWindows.Push(window);
                }
            }
        }

        private void OnCancel()
        {
            //Check if there is activewindow
            if (_activeWindows.Count > 0)
            {
                CloseActiveWindow(_activeWindows.Peek());
            }
            //If not open default window
            else
            {
                if (_defaultWindow == null)
                    return;
                
                OpenWindow(_defaultWindow);
            }
        }

        public void OpenWindow(IWindow window)
        {
            _activeWindows.Push(window);
            window.Active = true;
        }

        public void CloseWindow(IWindow window)
        {
            if (_activeWindows.Contains(window))
                CloseActiveWindow(window);
        }

        private void CloseActiveWindow(IWindow window)
        {
            IWindow _topWindow = _activeWindows.Pop();
            if (_topWindow != window)
                CloseActiveWindow(_topWindow);
            _topWindow.Active = false;
        }
    }
}
