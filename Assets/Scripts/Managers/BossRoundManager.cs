using System;
using UnityEngine;

public class BossRoundManager : Singleton<BossRoundManager>
{
    [SerializeField] private BossRoundListSO bossRoundListSO;

    private BossRoundSO currentBossRoundSO = null;
    public BossRoundSO CurrentBossRoundSO => currentBossRoundSO;

    public event Action OnBossRoundEntered;
    public event Action OnBossRoundExited;

    public void EnterBossRound()
    {
        if (currentBossRoundSO != null) return;
        SequenceManager.Instance.AddCoroutine(() =>
        {
            currentBossRoundSO = bossRoundListSO.GetRandomBossRoundSO();
            currentBossRoundSO.OnEnter();
            OnBossRoundEntered?.Invoke();
        });
    }

    public void ExitBossRound()
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