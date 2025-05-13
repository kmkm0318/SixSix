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

        GameManager.Instance.CurrentGameState = GameState.Shop;
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