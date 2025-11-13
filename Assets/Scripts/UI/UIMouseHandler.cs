using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class UIMouseHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform _rectTransform;
    private Image _image;
    private bool _isEntered = false;

    public RectTransform RectTransform => _rectTransform;
    public Image Image => _image;

    public event Action OnPointerExited;
    public event Action OnPointerEntered;
    public event Action OnPointerClicked;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isEntered) return;
        _isEntered = true;

        OnPointerEntered?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isEntered) return;
        _isEntered = false;

        OnPointerExited?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (eventData.clickCount > 1) return;

        OnPointerClicked?.Invoke();
    }
}