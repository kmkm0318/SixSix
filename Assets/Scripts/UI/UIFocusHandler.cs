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
public class UIFocusHandler : MonoBehaviour, IFocusable, IPointerClickHandler//, IPointerEnterHandler, IPointerExitHandler
{
    protected RectTransform RectTransform { get; private set; }
    protected Image Image { get; private set; }

    protected bool IsPointerOver { get; private set; } = false;

    public event Action OnPointerExited;
    public event Action OnPointerEntered;
    public event Action OnPointerClicked;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        Image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsPointerOver = true;
        OnPointerEntered?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsPointerOver = false;
        OnPointerExited?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (FocusManager.Instance) FocusManager.Instance.OnClick(this);
    }

    public void OnFocus()
    {
        OnPointerEnter(null);
    }

    public void OnUnfocus()
    {
        OnPointerExit(null);
    }

    public void OnInteract()
    {
        OnPointerClicked?.Invoke();
    }
}