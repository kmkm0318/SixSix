using System;

public class RoundClearManager : Singleton<RoundClearManager>
{
    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        RoundClearUI.Instance.OnRoundClearUIClosed += OnRoundClearUIClosed;
    }

    public void StartRoundClear()
    {
        GameManager.Instance.ChangeState(GameState.RoundClear);
    }

    private void OnRoundClearUIClosed()
    {
        ShopManager.Instance.StartShop();
    }

    public int GetRewardValue(RoundClearRewardType type)
    {
        return type switch
        {
            RoundClearRewardType.RoundNum => MoneyManager.Instance.RoundClearReward,
            RoundClearRewardType.PlayRemain => MoneyManager.Instance.PlayRemainReward,
            RoundClearRewardType.BossRound => MoneyManager.Instance.BossRoundReward,
            RoundClearRewardType.MoneyInterest => MoneyManager.Instance.MoneyInterestReward,
            _ => 0,
        };
    }
}

public enum RoundClearRewardType
{
    None,
    RoundNum,
    PlayRemain,
    BossRound,
    MoneyInterest,
}