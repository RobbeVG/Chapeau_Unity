using Reflex.Attributes;
using UnityEngine;

namespace Seacore.Game
{
    public class WindowManager : MonoBehaviour
    {
        [Inject]
        InputActionManager _inputActionManager = null;

        [SerializeField]
        GameObject _defaultWindow = null;

        GameObject[] _children;

        private void Awake()
        {
            _inputActionManager.OnWindowCancel += OnWindowCancel;
            //loop over all children of the GameObject to create array

            bool defaultActiveWindowIsChild = false;
            int childCount = transform.childCount;
            _children = new GameObject[childCount];
            for (int i = 0; i < childCount; i++)
            {
                _children[i] = transform.GetChild(i).gameObject;
                
                if (_children[i] == _defaultWindow)
                {
                    defaultActiveWindowIsChild = true;
                }
            }

            //Check if default active window is set and activate it
            if (_defaultWindow != null && defaultActiveWindowIsChild)
            {
                Debug.LogError("Default Active Window is not a child of the Window Manager", this);
            }
        }

        private void Start()
        {
            foreach (GameObject child in _children)
            {
                child.SetActive(false);
            }
        }

        private void OnWindowCancel()
        {
            if (_defaultWindow != null)
            {
                _defaultWindow.SetActive(!_defaultWindow.activeSelf);
            }
            
        }
    }
}
