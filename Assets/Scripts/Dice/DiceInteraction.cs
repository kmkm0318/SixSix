using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceInteraction : MonoBehaviour, IClickable
{
    public event Action OnMouseEntered;
    public event Action OnMouseExited;
    public event Action OnMouseClicked;
    public event Action<bool> OnIsMouseOverChanged;

    private Dice dice;

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

    public void Init(Dice dice)
    {
        this.dice = dice;

        RegisterEvents();

        if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            IsInteractable = true;
        }
        else
        {
            IsInteractable = false;
        }
    }

    private void OnDisable()
    {
        UnregisterEvents();
        IsMouseOver = false;
    }

    #region Events
    private void RegisterEvents()
    {
        PlayManager.Instance.OnPlayStarted += OnPlayStarted;
        PlayManager.Instance.OnPlayEnded += OnPlayEnded;
        RollManager.Instance.OnRollStarted += OnRollStarted;
        RollManager.Instance.OnRollCompleted += OnRollCompleted;
        ShopManager.Instance.OnShopStarted += OnShopStarted;
        ShopManager.Instance.OnShopEnded += OnShopEnded;
    }

    private void UnregisterEvents()
    {
        PlayManager.Instance.OnPlayStarted -= OnPlayStarted;
        PlayManager.Instance.OnPlayEnded -= OnPlayEnded;
        RollManager.Instance.OnRollStarted -= OnRollStarted;
        RollManager.Instance.OnRollCompleted -= OnRollCompleted;
        ShopManager.Instance.OnShopStarted -= OnShopStarted;
        ShopManager.Instance.OnShopEnded -= OnShopEnded;
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

    private void OnShopStarted()
    {
        if (dice is AvailityDice)
        {
            IsInteractable = true;
        }
    }

    private void OnShopEnded()
    {
        if (dice is AvailityDice)
        {
            IsInteractable = false;
        }
    }

    void OnMouseEnter()
    {
        IsMouseOver = true;

        if (!IsInteractable) return;
        if (Functions.IsPointerOverUIElement()) return;

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
        if (Functions.IsPointerOverUIElement()) return;

        OnMouseClicked?.Invoke();
    }
    #endregion
}
