using System;
using System.Collections;

public class RoundClearManager : Singleton<RoundClearManager>
{
    public event Action OnRoundClearStarted;
    public event Action OnRoundClearEnded;

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.RoundClear)
        {
            OnRoundClearStarted?.Invoke();
            SequenceManager.Instance.ExecuteLater(() => OnRoundClearEnded?.Invoke()); ;
        }
    }
}
