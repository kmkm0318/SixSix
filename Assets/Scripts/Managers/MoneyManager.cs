using System;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private int bonusMoney = 25;
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
    }

    private void OnBonusAchieved(BonusType type)
    {
        if (type == BonusType.Money)
        {
            Money += bonusMoney;
        }
    }
}
