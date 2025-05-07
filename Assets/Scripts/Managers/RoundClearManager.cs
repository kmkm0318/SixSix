using System;

public class RoundClearManager : Singleton<RoundClearManager>
{
    public event Action OnRoundClearStarted;
    public event Action OnRoundClearEnded;

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        RoundClearUI.Instance.OnRoundClearUIClosed += OnRoundClearUIClosed;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.RoundClear)
        {
            OnRoundClearStarted?.Invoke();
        }
    }

    private void OnRoundClearUIClosed()
    {
        OnRoundClearEnded?.Invoke();
    }

    public int GetRewardValue(RoundClearRewardType type)
    {
        return type switch
        {
            RoundClearRewardType.RoundNum => PlayerMoneyManager.Instance.RoundClearReward,
            RoundClearRewardType.PlayRemain => PlayerMoneyManager.Instance.PlayRemainReward,
            RoundClearRewardType.BossRound => PlayerMoneyManager.Instance.BossRoundReward,
            RoundClearRewardType.MoneyInterest => PlayerMoneyManager.Instance.MoneyInterestReward,
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