using System;
using UnityEngine;

public class RoundManager : Singleton<RoundManager>
{
    [SerializeField] private int clearRound = 25;
    public int ClearRound => clearRound;

    public event Action<int> OnRoundStarted;
    public event Action<int> OnRoundCleared;
    public event Action<int> OnRoundFailed;

    private int currentRound = 0;
    public int CurrentRound => currentRound;

    void Start()
    {
        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        ScoreManager.Instance.OnCurrentRoundScoreUpdated += OnCurrentRoundScoreUpdated;
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Round)
        {
            StartNextRound();
        }
    }

    private void OnCurrentRoundScoreUpdated(int score)
    {
        if (score <= 0) return;

        if (score >= ScoreManager.Instance.TargetRoundScore)
        {
            ClearCurrentRound();
        }
        else if (PlayManager.Instance.PlayRemain == 0)
        {
            FailCurrentRound();
        }
    }
    #endregion

    private void StartNextRound()
    {
        currentRound++;
        Debug.Log($"Round {currentRound} started.");
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
