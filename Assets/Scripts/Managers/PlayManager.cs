using System;
using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    [SerializeField] private int playMax;

    public event Action<int> OnPlayStarted;
    public event Action<int> OnPlayEnded;
    public event Action<int> OnPlayRemainChanged;

    private int currentPlayMax;
    private int playRemain = 0;
    public int PlayRemain
    {
        get => playRemain;
        set
        {
            if (playRemain == value) return;
            playRemain = value;
            OnPlayRemainChanged?.Invoke(playRemain);
        }
    }

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        playMax = DataContainer.Instance.CurrentDiceStat.defaultMaxPlay;
        currentPlayMax = playMax;
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ScoreManager.Instance.OnTargetRoundScoreUpdated += OnTargetRoundScoreUpdated;
        ScoreManager.Instance.OnCurrentRoundScoreUpdated += OnCurrentRoundScoreUpdated;
        EnhanceManager.Instance.OnDiceEnhanceStarted += OnDiceEnhanceStarted;
        EnhanceManager.Instance.OnHandEnhanceStarted += OnHandEnhanceStarted;
        EnhanceManager.Instance.OnHandEnhanceCompleted += OnHandEnhanceCompleted;
        BonusManager.Instance.OnBonusAchieved += OnBonusAchieved;
    }

    private void OnTargetRoundScoreUpdated(float score)
    {
        PlayRemain = currentPlayMax;
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

    private void OnBonusAchieved(BonusType type)
    {
        if (type == BonusType.PlayMax)
        {
            SetPlayMax(playMax + 1, false);
        }
    }
    #endregion

    public void SetPlayMax(int value, bool resetPlayRemain = true)
    {
        playMax = value;
        currentPlayMax = playMax;

        if (resetPlayRemain)
        {
            PlayRemain = playMax;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }

    public void SetCurrentPlayMax(int value = -1, bool resetPlayRemain = true)
    {
        if (value == -1) value = playMax;
        currentPlayMax = value;

        if (resetPlayRemain)
        {
            PlayRemain = currentPlayMax;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }
}
