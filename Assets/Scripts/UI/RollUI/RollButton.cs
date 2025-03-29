using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnButtonPressed;
    public event Action OnButtonReleased;

    private bool isActive = true;
    private bool isPressed = false;

    private void Start()
    {
        RollManager.Instance.OnRollStarted += () => isActive = false;
        RollManager.Instance.OnRollCompleted += () => isActive = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isActive || isPressed || eventData.button != PointerEventData.InputButton.Left) return;
        isPressed = true;
        OnButtonPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isActive || !isPressed || eventData.button != PointerEventData.InputButton.Left) return;
        isPressed = false;
        OnButtonReleased?.Invoke();
    }
}