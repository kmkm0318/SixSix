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
        var playerStat = DataContainer.Instance.CurrentPlayerStat;

        return type switch
        {
            RoundClearRewardType.RoundClear => playerStat.roundClearReward,
            RoundClearRewardType.BossRoundClear => RoundManager.Instance.IsBossRound ? playerStat.bossRoundReward : 0,
            RoundClearRewardType.PlayRemain => playerStat.playRemainReward * PlayManager.Instance.PlayRemain,
            RoundClearRewardType.MoneyInterest => Mathf.Min(playerStat.interestMax, MoneyManager.Instance.Money / playerStat.interestUnit * playerStat.interestPerUnit),
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