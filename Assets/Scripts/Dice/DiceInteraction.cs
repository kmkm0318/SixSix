using System;
using UnityEngine;

public class DiceInteraction : MonoBehaviour, IClickable
{
    public event Action OnMouseEntered;
    public event Action OnMouseExited;
    public event Action OnMouseClicked;

    private bool isInteractable = false;
    public bool IsInteractable
    {
        get => isInteractable;
        set
        {
            isInteractable = value;
            if (!value && isMouseOver)
            {
                OnMouseExited?.Invoke();
            }
        }
    }
    private bool isMouseOver = false;

    private void Start()
    {
        PlayManager.Instance.OnPlayStarted += OnPlayStarted;
        PlayManager.Instance.OnPlayEnded += OnPlayEnded;
        RollManager.Instance.OnRollStarted += OnRollStarted;
        RollManager.Instance.OnRollCompleted += OnRollCompleted;
    }

    private void OnPlayStarted(int obj)
    {
        IsInteractable = false;
    }

    private void OnPlayEnded(int obj)
    {
        IsInteractable = false;
    }

    private void OnRollStarted()
    {
        IsInteractable = false;
    }

    private void OnRollCompleted()
    {
        IsInteractable = RollManager.Instance.RollRemain > 0;
    }

    void OnMouseEnter()
    {
        if (!IsInteractable) return;

        isMouseOver = true;
        OnMouseEntered?.Invoke();
    }

    void OnMouseExit()
    {
        if (!IsInteractable) return;

        isMouseOver = false;
        OnMouseExited?.Invoke();
    }

    public void OnClick()
    {
        if (!IsInteractable) return;

        OnMouseClicked?.Invoke();
    }
}
