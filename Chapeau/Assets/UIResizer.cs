using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Seacore.UI
{
    [ExecuteInEditMode]
    public class UIResizer : MonoBehaviour
    {
        [Tooltip("The rect Transform of the content object to be set to")]
        [SerializeField] RectTransform _rectOfContent;
        [SerializeField] Vector2 minSize = new Vector2(0, 0);
        Transform _rootParent = null;

        void Awake()
        {
            _rootParent = gameObject.transform;
            while (_rootParent.parent)
            {
                _rootParent = _rootParent.parent;
            }
        }

        private void OnEnable()
        {
            if (!_rootParent)
                Awake();
        }


        private void Update()
        {
            if (Selection.activeGameObject == null)
                return;

            if (!Selection.activeGameObject.transform.IsChildOf(_rootParent))
                return;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(
                Mathf.Max(minSize.x, _rectOfContent.rect.width),
                Mathf.Max(minSize.y, _rectOfContent.rect.height)
            );
        }
    }
}
