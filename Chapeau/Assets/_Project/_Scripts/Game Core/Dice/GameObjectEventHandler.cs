using Seacore.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Seacore.Game
{
    public class GameObjectEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public Action<GameObject> OnHoverEnter = delegate { };
        public Action<GameObject> OnHoverExit = delegate { };
        public Action<GameObject> OnSelected = delegate { };
        public Action<GameObject> OnDeSelected = delegate { };

        public void OnSelect(BaseEventData eventData) => OnSelected.Invoke(gameObject);  
        public void OnDeselect(BaseEventData eventData) => OnDeSelected.Invoke(gameObject);
        public void OnPointerEnter(PointerEventData eventData) => OnHoverEnter.Invoke(gameObject);
        public void OnPointerExit(PointerEventData eventData) => OnHoverExit.Invoke(gameObject);
    }
}
