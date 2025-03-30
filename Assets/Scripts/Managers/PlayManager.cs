using System;
using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    [SerializeField] private int playMax = 3;

    public event Action<int> OnPlayStarted;
    public event Action<int> OnPlayEnded;

    private int playRemain = 0;

    private void StartPlay()
    {
        OnPlayStarted?.Invoke(playRemain);
    }

    private void EndPlay()
    {
        OnPlayEnded?.Invoke(playRemain);
    }
}
