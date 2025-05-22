using System.Collections.Generic;
using UnityEngine;

public class RoundClearManager : Singleton<RoundClearManager>
{
    [SerializeField] private List<RoundClearRewardType> defaultRewardList;

    public List<RoundClearRewardType> DefaultRewardList => defaultRewardList;

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        RoundClearUI.Instance.OnRoundClearUIClosed += OnRoundClearUIClosed;
    }

    private void OnRoundClearUIClosed()
    {
        ShopManager.Instance.StartShop();
    }

    public int GetRewardValue(RoundClearRewardType type)
    {
        return type switch
        {
            RoundClearRewardType.RoundClear => MoneyManager.Instance.RoundClearReward,
            RoundClearRewardType.BossRoundClear => MoneyManager.Instance.BossRoundReward,
            RoundClearRewardType.PlayRemain => MoneyManager.Instance.PlayRemainReward,
            RoundClearRewardType.MoneyInterest => MoneyManager.Instance.MoneyInterestReward,
            _ => 0,
        };
    }
}

public enum RoundClearRewardType
{
    None,
    RoundClear,
    BossRoundClear,
    PlayRemain,
    MoneyInterest,
}