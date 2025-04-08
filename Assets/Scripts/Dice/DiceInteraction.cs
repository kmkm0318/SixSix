using System;
using UnityEngine;

public class DiceInteraction : MonoBehaviour, IClickable
{
    public event Action OnMouseEntered;
    public event Action OnMouseExited;
    public event Action OnMouseClicked;
    public event Action<bool> OnIsMouseOverChanged;

    private bool isInteractable = false;
    public bool IsInteractable
    {
        get => isInteractable;
        set
        {
            if (isInteractable == value) return;
            isInteractable = value;
            if (value && isMouseOver)
            {
                OnMouseEntered?.Invoke();
            }
            else if (!value && isMouseOver)
            {
                OnMouseExited?.Invoke();
            }
        }
    }
    private bool isMouseOver = false;
    public bool IsMouseOver
    {
        get => isMouseOver;
        private set
        {
            if (isMouseOver == value) return;
            isMouseOver = value;
            OnIsMouseOverChanged?.Invoke(isMouseOver);
        }
    }

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
        IsMouseOver = true;

        if (!IsInteractable) return;

        OnMouseEntered?.Invoke();
    }

    void OnMouseExit()
    {
        IsMouseOver = false;

        if (!IsInteractable) return;

        OnMouseExited?.Invoke();
    }

    public void OnClick()
    {
        if (!IsInteractable) return;

        OnMouseClicked?.Invoke();
    }
}
