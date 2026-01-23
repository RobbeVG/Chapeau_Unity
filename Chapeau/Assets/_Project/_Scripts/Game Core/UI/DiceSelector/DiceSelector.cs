using DG.Tweening;
using Seacore;
using Seacore.Common;
using System;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using static Seacore.Common.Die;

[RequireComponent(typeof(Selectable))]
public class DiceSelector : MonoBehaviour, IDieValueGetter<Faces>, ISelectHandler, IMoveHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Visual Settings")]
    [SerializeField]
    private DiceDisplayValues diceValues = null;
    [SerializeField]
    private DiceSelectorConfig config = null;

    [Header("Reference Objects")]
    [SerializeField]
    private GameObject upArrow = null;
    [SerializeField]
    private GameObject downArrow = null;
    [SerializeField]
    private Image imageIcon = null;


    public event IDieValueGetter<Faces>.OnEditDeclareRollHandler OnEditValue;

    [field: SerializeField]
    public TypedDieData<Faces> SelectedFace { get; } = new TypedDieData<Faces>(Faces.None, noneExclusive: false);

    private Selectable selectable = null;
    
    private Tween tweenUpArrow = null;
    private Tween tweenDownArrow = null;

    private void OnValidate()
    {
        SetIconSprite(SelectedFace.Value);
    }

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
        tweenUpArrow = InitYOffsetTween(upArrow, config.EndYOffset);
        tweenDownArrow = InitYOffsetTween(downArrow, -config.EndYOffset);
    }

    public Faces Value => SelectedFace.Value;

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
        tweenUpArrow.PlayForward();
        tweenDownArrow.PlayForward();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        tweenUpArrow.PlayBackwards();
        tweenDownArrow.PlayBackwards();
    }

    //Maybe global function for all dice selectors?
    public void OnPointerEnter(PointerEventData eventData)
    {
        selectable.Select();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null, eventData);
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

    private void SwipeUp()
    {
        upArrow.transform.DOPunchPosition(new Vector3(0.0f, config.NudgeAmount, 0.0f), config.NudgeDuration);
        SetIconSprite(SelectedFace.Next);
        OnEditValue?.Invoke(this);
    }
    private void SwipeDown()
    {
        downArrow.transform.DOPunchPosition(new Vector3(0.0f, -config.NudgeAmount, 0.0f), config.NudgeDuration);
        SetIconSprite(SelectedFace.Previous);
        OnEditValue?.Invoke(this);
    }
    private Tween InitYOffsetTween(GameObject gm, float offset)
    {
        Tween tween = gm.transform.DOLocalMoveY(offset, config.FadeDuration);
        tween.SetEase(config.EaseType);
        tween.SetAutoKill(false);
        tween.Pause();
        return tween;
    }
    private void SetIconSprite(Die.Faces face)
    {
        if (imageIcon == null)
            Debug.LogWarning("No attached icon component", this);
        else
        {
            imageIcon.sprite = diceValues.GetFaceSprite(SelectedFace.Value);
            if (SelectedFace.Value == Die.Faces.None)
                imageIcon.enabled = false;
            else
                imageIcon.enabled = true;
        }
    }

}
