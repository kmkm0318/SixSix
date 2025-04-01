using System;
using System.Collections;

public enum GameState
{
    None,
    Loading,
    Round,
    RoundClear,
    RoundFail,
    Shop,
    GameClear,
}

public class GameManager : Singleton<GameManager>
{
    public event Action<GameState> OnGameStateChanged;

    private GameState currentGameState = GameState.None;
    public GameState CurrentGameState
    {
        get => currentGameState;
        private set
        {
            if (currentGameState == value) return;
            currentGameState = value;
            OnGameStateChanged?.Invoke(currentGameState);
        }
    }

    #region StartGame
    private void Start()
    {
        StartCoroutine(StartGame());
        RegisterEvents();
    }

    private IEnumerator StartGame()
    {
        yield return null;

        CurrentGameState = GameState.Loading;
    }
    #endregion

    #region RegisterEvents
    private void RegisterEvents()
    {
        PlayerDiceManager.Instance.OnFirstDiceGenerated += OnFirstDiceGenerated;
        RoundManager.Instance.OnRoundCleared += OnRoundCleared;
        RoundManager.Instance.OnRoundFailed += OnRoundFailed;
    }

    private void OnFirstDiceGenerated()
    {
        CurrentGameState = GameState.Round;
    }

    private void OnRoundCleared(int currentRound)
    {
        if (currentRound == RoundManager.Instance.ClearRound)
        {
            CurrentGameState = GameState.GameClear;
        }
        else
        {
            CurrentGameState = GameState.RoundClear;
        }
    }

    private void OnRoundFailed(int currentRound)
    {
        CurrentGameState = GameState.RoundFail;
    }
    #endregion
}
