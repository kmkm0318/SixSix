using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMouseHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Image image;
    private bool isEntered = false;

    public RectTransform RectTransform => rectTransform;
    public Image Image => image;

    public event Action OnPointerExited;
    public event Action OnPointerEntered;
    public event Action OnPointerClicked;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isEntered) return;
        isEntered = true;

        OnPointerEntered?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isEntered) return;
        isEntered = false;

        OnPointerExited?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (eventData.clickCount > 1) return;

        OnPointerClicked?.Invoke();
    }
}