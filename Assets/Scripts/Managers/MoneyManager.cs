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
    public int BossRoundReward => RoundManager.Instance.IsBossRound ? bossRoundReward : 0;

    public event Action<int> OnMoneyChanged;
    public event Action<int> OnMoneyAdded;
    public event Action<int> OnMoneyRemoved;

    private void Start()
    {
        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnAvailityDicePurchaseAttempted += OnPurchaseAttempted;
        ShopManager.Instance.OnAvailityDiceSelled += OnAvailityDiceSelled;
        ShopManager.Instance.OnHandEnhancePurchaseAttempted += OnHandEnhancePurchaseAttempted;
        ShopManager.Instance.OnPlayDiceEnhancePurchaseAttempted += OnPlayDiceEnhancePurchaseAttempted;
    }

    private void OnPurchaseAttempted(AvailityDiceSO sO, PurchaseResult result)
    {
        if (sO == null) return;

        if (result == PurchaseResult.Success)
        {
            Money -= sO.price;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }

    private void OnAvailityDiceSelled(AvailityDiceSO sO)
    {
        if (sO == null) return;

        Money += sO.SellPrice;
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private void OnHandEnhancePurchaseAttempted(HandEnhancePurchaseContext context, PurchaseResult result)
    {
        if (result == PurchaseResult.Success)
        {
            Money -= context.Price;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }

    private void OnPlayDiceEnhancePurchaseAttempted(DiceEnhancePurchaseContext context, PurchaseResult result)
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
