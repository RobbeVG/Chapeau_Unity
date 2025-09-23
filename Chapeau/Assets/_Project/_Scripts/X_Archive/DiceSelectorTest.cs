using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceSelectorTest : MonoBehaviour
{
    Seacore.Common.Outline outline;
    Selectable selectable;

    public void OnCancel(BaseEventData eventData)
    {
        Debug.Log("Cancel on " + gameObject.name);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("Submit on " + gameObject.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.GetComponent<Seacore.Common.Outline>();
        if (!outline)
            Debug.LogError("fout");
        outline.OutlineColor = Color.green;
        selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (outline && selectable)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        }
    }
}
