using System;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    private int money = 0;

    public int Money
    {
        get => money;
        set
        {
            if (money == value) return;
            if (value > money)
            {
                OnMoneyAdded?.Invoke(value - money);
            }
            else
            {
                OnMoneyRemoved?.Invoke(money - value);
            }
            money = Mathf.Clamp(value, 0, int.MaxValue);
            OnMoneyChanged?.Invoke(money);
        }
    }

    public event Action<int> OnMoneyChanged;
    public event Action<int> OnMoneyAdded;
    public event Action<int> OnMoneyRemoved;

    private void Start()
    {
        RegisterEvents();
        Money = DataContainer.Instance.CurrentPlayerStat.startMoney;
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnAbilityDicePurchaseAttempted += OnPurchaseAttempted;
        ShopManager.Instance.OnAbilityDiceSelled += OnAbilityDiceSelled;
        ShopManager.Instance.OnGambleDiceSelled += OnGambleDiceSelled;
        ShopManager.Instance.OnGambleDicePurchaseAttempted += OnGambleDicePurchaseAttempted;
        ShopManager.Instance.OnEnhancePurchaseAttempted += OnEnhancePurchaseAttempted;
    }

    private void OnPurchaseAttempted(AbilityDiceSO sO, PurchaseResult result)
    {
        if (sO == null) return;

        if (result == PurchaseResult.Success)
        {
            Money -= sO.price;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }

    private void OnAbilityDiceSelled(AbilityDiceSO sO)
    {
        if (sO == null) return;

        Money += sO.SellPrice;
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private void OnGambleDiceSelled(GambleDiceSO sO)
    {
        if (sO == null) return;

        Money += sO.SellPrice;
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private void OnGambleDicePurchaseAttempted(GambleDiceSO sO, PurchaseResult result)
    {
        if (sO == null) return;

        if (result == PurchaseResult.Success)
        {
            Money -= sO.price;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }

    private void OnEnhancePurchaseAttempted(EnhancePurchaseContext context, PurchaseResult result)
    {
        if (result == PurchaseResult.Success)
        {
            Money -= context.Price;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }
    #endregion

    public void AddMoney(int value, bool apply = false)
    {
        Money += value;

        if (apply)
        {
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }
}
