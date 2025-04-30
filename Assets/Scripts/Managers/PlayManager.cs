using System;
using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    [SerializeField] private int playMax = 3;
    public int PlayMax => playMax;

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

    #region RegisterEvents
    private void RegisterEvents()
    {
        ScoreManager.Instance.OnTargetRoundScoreUpdated += OnTargetRoundScoreUpdated;
        ScoreManager.Instance.OnCurrentRoundScoreUpdated += OnCurrentRoundScoreUpdated;
        EnhanceManager.Instance.OnDiceEnhanceStarted += OnDiceEnhanceStarted;
        EnhanceManager.Instance.OnHandEnhanceStarted += OnHandEnhanceStarted;
        EnhanceManager.Instance.OnHandEnhanceCompleted += OnHandEnhanceCompleted;
    }

    private void OnTargetRoundScoreUpdated(float score)
    {
        PlayRemain = playMax;
        OnPlayStarted?.Invoke(PlayRemain);
    }

    private void OnCurrentRoundScoreUpdated(float score)
    {
        PlayRemain--;

        OnPlayEnded?.Invoke(PlayRemain);

        if (PlayRemain > 0 && score < ScoreManager.Instance.TargetRoundScore)
        {
            OnPlayStarted?.Invoke(PlayRemain);
        }
    }

    private void OnDiceEnhanceStarted()
    {
        PlayRemain = 0;
    }

    private void OnHandEnhanceStarted()
    {
        PlayRemain = 1;
    }

    private void OnHandEnhanceCompleted()
    {
        PlayRemain = 0;
    }
    #endregion

    public void SetPlayMax(int value, bool resetPlayRemain = true)
    {
        playMax = value;
        if (resetPlayRemain)
        {
            PlayRemain = playMax;
        }
    }
}
