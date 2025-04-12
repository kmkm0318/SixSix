using System;
using UnityEngine;

public class PlayerMoneyManager : Singleton<PlayerMoneyManager>
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

    public event Action<int> OnMoneyChanged;
    private int money = 0;
    public int Money
    {
        get => money;
        private set
        {
            if (money == value) return;
            money = value;
            OnMoneyChanged?.Invoke(money);
        }
    }

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        BonusManager.Instance.OnBonusAchieved += OnBonusAchieved;
        RoundClearUI.Instance.OnRewardTriggered += OnRoundClearRewardTriggered;
    }

    private void OnRoundClearRewardTriggered(int value)
    {
        Money += value;
    }

    private void OnBonusAchieved(BonusType type)
    {
        if (type == BonusType.Money)
        {
            Money += bonusMoney;
        }
    }
}
