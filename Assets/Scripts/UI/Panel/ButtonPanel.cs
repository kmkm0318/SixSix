using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Shadow))]
public class ButtonPanel : TextPanel, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector2 _pressedOffset = new(0, -4);
    private Button _button;
    private Shadow _shadow;
    private Vector2 _originalPos = Vector2.zero;
    private bool _isPressed = false;

    public event Action OnClick;
    public event Action OnButtonDown;
    public event Action OnButtonUp;
    public event Action OnPointerEntered;
    public event Action OnPointerExited;

    protected override void Awake()
    {
        base.Awake();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => OnClick?.Invoke());

        _shadow = GetComponent<Shadow>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (_isPressed || eventData.button != PointerEventData.InputButton.Left) return;
        _isPressed = true;

        HandlePos();
        AudioManager.Instance.PlaySFX(SFXType.ButtonDown);

        OnButtonDown?.Invoke();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (!_isPressed || eventData.button != PointerEventData.InputButton.Left) return;
        _isPressed = false;

        HandlePos();
        AudioManager.Instance.PlaySFX(SFXType.ButtonUp);

        OnButtonUp?.Invoke();
    }

    private void HandlePos()
    {
        if (_isPressed)
        {
            _shadow.enabled = false;
            if (_originalPos == Vector2.zero)
            {
                _originalPos = _rectTransform.anchoredPosition;
            }
            _rectTransform.anchoredPosition = _originalPos + _pressedOffset;
        }
        else
        {
            _shadow.enabled = true;
            _rectTransform.anchoredPosition = _originalPos;
        }
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEntered?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExited?.Invoke();
    }
}