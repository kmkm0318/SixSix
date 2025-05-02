using System;
using UnityEngine;

public class RoundManager : Singleton<RoundManager>
{
    [SerializeField] private int clearRound = 36;
    public int ClearRound => clearRound;
    [SerializeField] private int bossRoundInterval = 6;
    public int BossRoundInterval => bossRoundInterval;
    public bool IsBossRound => (CurrentRound % bossRoundInterval) == 0 && CurrentRound != 0;

    public event Action<int> OnRoundStarted;
    public event Action<int> OnRoundCleared;
    public event Action<int> OnRoundFailed;
    public event Action<int> OnCurrentRoundChanged;

    private int currentRound = 0;
    public int CurrentRound
    {
        get => currentRound;
        private set
        {
            if (currentRound == value) return;
            currentRound = value;
            OnCurrentRoundChanged?.Invoke(currentRound);
        }
    }

    void Start()
    {
        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        PlayManager.Instance.OnPlayEnded += OnPlayEnded;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Round)
        {
            StartNextRound();
        }
    }

    private void OnPlayEnded(int playRemain)
    {
        if (ScoreManager.Instance.CurrentRoundScore >= ScoreManager.Instance.TargetRoundScore)
        {
            ClearCurrentRound();
        }
        else if (playRemain == 0)
        {
            FailCurrentRound();
        }
    }
    #endregion

    private void StartNextRound()
    {
        CurrentRound++;
        OnRoundStarted?.Invoke(CurrentRound);
    }

    private void ClearCurrentRound()
    {
        OnRoundCleared?.Invoke(CurrentRound);
    }

    private void FailCurrentRound()
    {
        OnRoundFailed?.Invoke(CurrentRound);
    }
}
