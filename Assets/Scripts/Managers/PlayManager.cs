using System;
using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    [SerializeField] private int playMax = 3;

    public event Action<int> OnPlayStarted;
    public event Action<int> OnPlayEnded;
    public event Action<int> OnPlayRemainChanged;

    private int playRemain = 0;
    public int PlayRemain
    {
        get => playRemain;
        private set
        {
            if (playRemain == value) return;
            playRemain = value;
            OnPlayRemainChanged?.Invoke(playRemain);
        }
    }

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        ScoreManager.Instance.OnTargetRoundScoreUpdated += OnTargetRoundScoreUpdated;
        ScoreManager.Instance.OnCurrentRoundScoreUpdated += OnCurrentRoundScoreUpdated;
    }

    private void OnTargetRoundScoreUpdated(int score)
    {
        PlayRemain = playMax;
        OnPlayStarted?.Invoke(PlayRemain);
    }

    private void OnCurrentRoundScoreUpdated(int score)
    {
        if (score <= 0) return;
        PlayRemain--;

        OnPlayEnded?.Invoke(PlayRemain);

        if (PlayRemain > 0 && score < ScoreManager.Instance.TargetRoundScore)
        {
            OnPlayStarted?.Invoke(PlayRemain);
        }
    }
}
