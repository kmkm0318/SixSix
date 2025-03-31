using System;
using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    [SerializeField] private int playMax = 3;

    public event Action<int> OnPlayStarted;
    public event Action<int> OnPlayEnded;

    private int playRemain = 0;
    public int PlayRemain => playRemain;

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        RoundManager.Instance.OnRoundStarted += OnRoundStarted;
        ScoreManager.Instance.OnCurrentRoundScoreUpdated += OnCurrentRoundScoreUpdated;
    }

    private void OnRoundStarted(int currentRound)
    {
        playRemain = playMax;
        OnPlayStarted?.Invoke(playRemain);
    }

    private void OnCurrentRoundScoreUpdated(int score)
    {
        if (score <= 0) return;
        playRemain--;

        OnPlayEnded?.Invoke(playRemain);

        if (playRemain > 0 && score < ScoreManager.Instance.TargetRoundScore)
        {
            OnPlayStarted?.Invoke(playRemain);
        }
    }
}
