using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI 마우스 핸들러 컴포넌트
/// 마우스 포인터가 UI 요소 위에 진입, 이탈, 클릭했을 때 이벤트를 발생시킨다.
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class UIMouseHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (eventData.clickCount > 1) return;

        OnPointerClicked?.Invoke();
    }
}