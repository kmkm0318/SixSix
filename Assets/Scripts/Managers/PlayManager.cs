using System;
using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    [SerializeField] private int playMax = 3;

    public event Action<int> OnPlayStarted;
    public event Action<int> OnPlayEnded;
    public event Action OnHandPlayed;

    private int playRemain = 0;
    public int PlayRemain => playRemain;

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        RoundManager.Instance.OnRoundStarted += OnRoundStarted;
        RoundManager.Instance.OnRoundCleared += OnRoundCleared;
        RoundManager.Instance.OnRoundFailed += OnRoundFailed;
    }

    private void OnRoundStarted(int currentRound)
    {
        InitPlay();
    }

    private void OnRoundCleared(int currentRound)
    {
        throw new NotImplementedException();
    }

    private void OnRoundFailed(int currentRound)
    {
        throw new NotImplementedException();
    }

    private void InitPlay()
    {
        playRemain = playMax;
        StartNextPlay();
    }

    private void StartNextPlay()
    {
        OnPlayStarted?.Invoke(playRemain);
    }

    private void EndCurrentPlay()
    {
        playRemain--;
        OnPlayEnded?.Invoke(playRemain);
    }
}
