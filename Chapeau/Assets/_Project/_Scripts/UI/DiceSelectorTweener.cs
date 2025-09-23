using DG.Tweening;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class DiceSelectorTweener : MonoBehaviour, ISelectHandler, IMoveHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    GameObject upArrow = null;
    [SerializeField]
    GameObject downArrow = null;

    [SerializeField]
    bool TogglePointerHandler = true;

    [SerializeField]
    Ease easeType = Ease.InOutSine;
    [SerializeField]
    int nudgeAmount = 5;
    [SerializeField]
    float nudgeDuration = 0.1f;
    [SerializeField]
    float fadeDuration = 0.3f;
    [SerializeField]
    float endYOffset = 75f;

    Selectable selectable = null;
    Tween[] tweens = new Tween[2];

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
        if (selectable == null)
            Debug.LogWarning($"No Selectable component found on {gameObject.name}.", gameObject);
        if (upArrow == null)
            Debug.LogWarning($"No upArrow assigned on {gameObject.name}.", gameObject);
        if (downArrow == null)
            Debug.LogWarning($"No downArrow assigned on {gameObject.name}.", gameObject);
    }

    private void Start()
    {
        tweens[0] = upArrow.transform.DOLocalMoveY(endYOffset, fadeDuration);
        tweens[1] = downArrow.transform.DOLocalMoveY(-endYOffset, fadeDuration);

        foreach (Tween tween in tweens)
        {
            tween.SetEase(easeType);
            tween.SetAutoKill(false);
            tween.Pause();
        }
    }

    private void SwipeUp()
    {
        upArrow.transform.DOPunchPosition(new Vector3(0.0f, nudgeAmount, 0.0f), nudgeDuration);

    }

    private void SwipeDown()
    {
        downArrow.transform.DOPunchPosition(new Vector3(0.0f, -nudgeAmount, 0.0f), nudgeDuration); ;

    }


    public void OnMove(AxisEventData eventData)
    {
        MoveDirection moveDir = eventData.moveDir;
        switch (moveDir)
        {
            case MoveDirection.Up:
                SwipeUp();
                break;
            case MoveDirection.Down:
                SwipeDown();
                break;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        foreach (Tween tween in tweens)
        {
            tween.PlayForward();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        foreach (Tween tween in tweens)
        {
            tween.PlayBackwards();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSelect(eventData);
        //selectable.Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnDeselect(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerPress == gameObject)
        {
            if (eventData.position.y > transform.position.y)
                SwipeUp();
            else
                SwipeDown();
        }

        
    }
}
