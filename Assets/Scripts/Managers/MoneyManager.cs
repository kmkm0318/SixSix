using System;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private int bonusMoney = 25;
    [SerializeField] private int roundclearReward = 3;
    [SerializeField] private int playRemainReward = 1;
    [SerializeField] private int interestUnit = 10;
    [SerializeField] private int interestPerUnit = 1;
    [SerializeField] private int interestMax = 5;
    [SerializeField] private int bossRoundReward = 3;
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
    public int BonusMoney => bonusMoney;
    public int RoundClearReward => roundclearReward;
    public int PlayRemainReward => playRemainReward * PlayManager.Instance.PlayRemain;
    public int MoneyInterestReward => Mathf.Min(interestMax, Money / interestUnit * interestPerUnit);
    public int InterestMax => interestMax;
    public int BossRoundReward => RoundManager.Instance.IsBossRound ? bossRoundReward : 0;

    public event Action<int> OnMoneyChanged;
    public event Action<int> OnMoneyAdded;
    public event Action<int> OnMoneyRemoved;

    private void Start()
    {
        RegisterEvents();
        Money = DataContainer.Instance.CurrentDiceStat.defaultStartMoney;
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
