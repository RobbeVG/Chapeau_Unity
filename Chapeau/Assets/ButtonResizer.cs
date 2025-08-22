using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Seacore.UI
{
    [ExecuteInEditMode]
    public class ButtonResizer : MonoBehaviour
    {
        [SerializeField] RectTransform _rectOfContent;


        [SerializeField]  Vector2 minSize = new Vector2(0, 0);


        private void Update()
        {
            if (!(Selection.Contains(gameObject) || Selection.activeGameObject.transform.IsChildOf(gameObject.transform)))
                return;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(
                Mathf.Max(minSize.x, _rectOfContent.rect.width),
                Mathf.Max(minSize.y, _rectOfContent.rect.height)
            );
        }
    }
}
