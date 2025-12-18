using UnityEngine;
using UnityEngine.EventSystems;



namespace Seacore.Game
{
    [RequireComponent(typeof(Common.Outline))]
    public class DiceVisuals : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        private Common.Outline outline;

        private void Awake()
        {
            outline = gameObject.GetComponent<Seacore.Common.Outline>();
            if (!outline)
                Debug.LogError("No outline component on dice", this);
        }
        void Start()
        {
            outline.OutlineColor = Color.green;
            if (gameObject != EventSystem.current.currentSelectedGameObject) //Check if currently selected
                outline.enabled = false;
        }

        public void OnSelect(BaseEventData eventData)
        {
            outline.enabled = true;
        }
        public void OnDeselect(BaseEventData eventData)
        {
            outline.enabled = false;
        }
        public void OnSubmit(BaseEventData eventData)
        {
            Debug.Log("Submit on " + gameObject.name);
        }
    }
}
