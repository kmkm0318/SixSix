using System;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private int bonusMoney = 25;
    [SerializeField] private int roundclearReward = 3;
    public int RoundClearReward => roundclearReward;
    [SerializeField] private int playRemainReward = 1;
    public int PlayRemainReward => playRemainReward * PlayManager.Instance.PlayRemain;
    [SerializeField] private int interestUnit = 10;
    [SerializeField] private int interestPerUnit = 1;
    [SerializeField] private int interestMax = 5;
    public int MoneyInterestReward => Mathf.Min(interestMax, Money / interestUnit * interestPerUnit);
    [SerializeField] private int bossRoundReward = 3;
    public int BossRoundReward => RoundManager.Instance.IsBossRound ? bossRoundReward : 0;

    public event Action<int> OnMoneyChanged;

    private int money = 0;
    public int Money
    {
        get => money;
        set
        {
            if (money == value) return;
            if (value < 0)
            {
                value = 0;
            }
            else if (value > int.MaxValue)
            {
                value = int.MaxValue;
            }
            money = value;
            OnMoneyChanged?.Invoke(money);
        }
    }

    private void Start()
    {
        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        BonusManager.Instance.OnBonusAchieved += OnBonusAchieved;
        RoundClearUI.Instance.OnRewardTriggered += OnRoundClearRewardTriggered;
        ShopManager.Instance.OnAvailityDicePurchaseAttempted += OnPurchaseAttempted;
        ShopManager.Instance.OnAvailityDiceSelled += OnAvailityDiceSelled;
        ShopManager.Instance.OnHandEnhancePurchaseAttempted += OnHandEnhancePurchaseAttempted;
        ShopManager.Instance.OnPlayDiceEnhancePurchaseAttempted += OnPlayDiceEnhancePurchaseAttempted;
    }

    private void OnBonusAchieved(BonusType type)
    {
        if (type == BonusType.Money)
        {
            SequenceManager.Instance.AddCoroutine(() =>
            {
                Money += bonusMoney;
                SequenceManager.Instance.ApplyParallelCoroutine();
            });
        }
    }

    private void OnRoundClearRewardTriggered(int value)
    {
        Money += value;
        SequenceManager.Instance.ApplyParallelCoroutine();
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
}
