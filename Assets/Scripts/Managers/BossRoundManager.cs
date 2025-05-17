using System;
using UnityEngine;

public class BossRoundManager : Singleton<BossRoundManager>
{
    [SerializeField] private BossRoundListSO bossRoundListSO;

    public event Action OnBossRoundEntered;
    public event Action OnBossRoundExited;

    private BossRoundSO currentBossRoundSO = null;
    public BossRoundSO CurrentBossRoundSO => currentBossRoundSO;

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Round, OnRoundStarted, OnRoundCleared);
    }

    private void OnRoundStarted()
    {
        if (RoundManager.Instance.IsBossRound)
        {
            EnterBossRound();
        }
    }

    private void OnRoundCleared()
    {
        if (currentBossRoundSO != null)
        {
            ExitBossRound();
        }
    }

    private void EnterBossRound()
    {
        if (currentBossRoundSO != null) return;
        SequenceManager.Instance.AddCoroutine(() =>
        {
            currentBossRoundSO = bossRoundListSO.GetRandomBossRoundSO();
            currentBossRoundSO.OnEnter();
            OnBossRoundEntered?.Invoke();
        });
    }

    private void ExitBossRound()
    {
        if (currentBossRoundSO == null) return;
        SequenceManager.Instance.AddCoroutine(() =>
        {
            currentBossRoundSO.OnExit();
            currentBossRoundSO = null;
            OnBossRoundExited?.Invoke();
        });
    }
}