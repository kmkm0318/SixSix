using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPanel : TextPanel, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Vector2 pressedOffset = new(0, -4);

    private Button button;
    private Vector2 originalPos = Vector2.zero;
    private bool isPressed = false;

    public event Action OnClick;
    public event Action OnButtonDown;
    public event Action OnButtonUp;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClick?.Invoke());
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) return;
        if (isPressed || eventData.button != PointerEventData.InputButton.Left) return;
        isPressed = true;

        HandlePos();
        AudioManager.Instance.PlaySFX(SFXType.ButtonDown);

        OnButtonDown?.Invoke();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (!button.interactable) return;
        if (!isPressed || eventData.button != PointerEventData.InputButton.Left) return;
        isPressed = false;

        HandlePos();
        AudioManager.Instance.PlaySFX(SFXType.ButtonUp);

        OnButtonUp?.Invoke();
    }

    private void HandlePos()
    {
        if (isPressed)
        {
            shadow.enabled = false;
            if (originalPos == Vector2.zero)
            {
                originalPos = rectTransform.anchoredPosition;
            }
            rectTransform.anchoredPosition = originalPos + pressedOffset;
        }
        else
        {
            shadow.enabled = true;
            rectTransform.anchoredPosition = originalPos;
        }
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
}