using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isActive = false;
    private bool isPressed = false;

    public event Action OnButtonPressed;
    public event Action OnButtonReleased;

    #region RegisterEvents
    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted, OnPlayEnded);
        GameManager.Instance.RegisterEvent(GameState.Roll, OnRollStarted, OnRollCompleted);
        GameManager.Instance.RegisterEvent(GameState.Enhance, OnEnhanceStarted, OnEnhanceCompleted);
    }

    private void OnPlayStarted()
    {
        isActive = true;
    }

    private void OnPlayEnded()
    {
        isActive = false;
    }

    private void OnRollStarted()
    {
        isActive = false;
    }

    private void OnRollCompleted()
    {
        isActive = RollManager.Instance.RollRemain > 0;
    }

    private void OnEnhanceStarted()
    {
        isActive = true;
    }

    private void OnEnhanceCompleted()
    {
        isActive = false;
    }
    #endregion

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