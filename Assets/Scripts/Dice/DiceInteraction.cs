using System;
using UnityEngine;

public class DiceInteraction : MonoBehaviour, IClickable
{
    public event Action OnMouseEntered;
    public event Action OnMouseExited;
    public event Action OnMouseClicked;

    private bool isInteractable = true;
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

    void OnMouseEnter()
    {
        if (!isInteractable) return;

        isMouseOver = true;
        OnMouseEntered?.Invoke();
    }

    void OnMouseExit()
    {
        if (!isInteractable) return;

        isMouseOver = false;
        OnMouseExited?.Invoke();
    }

    public void OnClick()
    {
        if (!isInteractable) return;

        OnMouseClicked?.Invoke();
    }
}
