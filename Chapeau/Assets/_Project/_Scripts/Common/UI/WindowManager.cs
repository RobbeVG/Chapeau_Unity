using Reflex.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore.Common
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("If no window is present, open this on cancel event - [escape]")]
        private Window _defaultWindow = null;

        private Stack<IWindow> _activeWindows = new Stack<IWindow>();

        private void Start()
        {
            foreach (IWindow window in GetComponentsInChildren<IWindow>(true))
            {
                if (window.Active)
                {
                    _activeWindows.Push(window);
                }
            }
        }

        public void OnCancel()
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
