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
        UnityEngine.Debug.Log("RoundClearUIClosed");
    }
}

public enum RoundClearRewardType
{
    None,
    RoundNum,
    PlayRemain,
    MoneyInterest,
}