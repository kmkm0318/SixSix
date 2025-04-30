using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnButtonPressed;
    public event Action OnButtonReleased;

    private bool isActive = false;
    private bool isPressed = false;

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        PlayManager.Instance.OnPlayStarted += (_) => SequenceManager.Instance.AddCoroutine(() => { isActive = true; });
        PlayManager.Instance.OnPlayEnded += (_) => isActive = false;

        RollManager.Instance.OnRollStarted += () => isActive = false;
        RollManager.Instance.OnRollCompleted += () => isActive = RollManager.Instance.RollRemain > 0;

        EnhanceManager.Instance.OnDiceEnhanceStarted += () => isActive = true;
        EnhanceManager.Instance.OnDiceEnhanceCompleted += () => isActive = false;
        EnhanceManager.Instance.OnHandEnhanceStarted += () => isActive = true;
        EnhanceManager.Instance.OnHandEnhanceCompleted += () => isActive = false;
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