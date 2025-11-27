using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI 포커스 핸들러 클래스
/// 마우스 오버 및 클릭 이벤트를 처리한다
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class UIFocusHandler : MonoBehaviour, IFocusable, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected RectTransform RectTransform { get; private set; }
    protected Image Image { get; private set; }

    protected bool IsFocusing { get; private set; } = false;

    public event Action OnFocused;
    public event Action OnUnfocused;
    public event Action OnInteracted;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        Image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (FocusManager.Instance) FocusManager.Instance.SetFocus(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (FocusManager.Instance) FocusManager.Instance.UnsetFocus(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (FocusManager.Instance) FocusManager.Instance.OnClick(this);
    }

    public void OnFocus()
    {
        IsFocusing = true;
        OnFocused?.Invoke();
    }

    public void OnUnfocus()
    {
        IsFocusing = false;
        OnUnfocused?.Invoke();
    }

    public void OnInteract()
    {
        OnInteracted?.Invoke();
    }
}