using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
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

    public event Action<GameState> OnGameStateChanged;

    private Dictionary<GameState, List<Func<bool>>> stateCheckTaskDictionary = new();
    private GameState currentGameState = GameState.None;
    public GameState CurrentGameState => currentGameState;

    #region StartGame
    private void Start()
    {
        StartCoroutine(StartGame());
        RegisterEvents();
    }

    private IEnumerator StartGame()
    {
        yield return null;

        ChangeGameState(GameState.Loading);
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
        ChangeGameState(GameState.Round);
    }

    private void OnRoundCleared(int currentRound)
    {
        if (currentRound == RoundManager.Instance.ClearRound)
        {
            ChangeGameState(GameState.GameClear);
        }
        else
        {
            ChangeGameState(GameState.RoundClear);
        }
    }

    private void OnRoundFailed(int currentRound)
    {
        ChangeGameState(GameState.RoundFail);
    }
    #endregion

    #region CheckTask
    public void RegisterStateCheckTask(GameState gameState, Func<bool> checkTask)
    {
        if (stateCheckTaskDictionary.ContainsKey(gameState))
        {
            stateCheckTaskDictionary[gameState].Add(checkTask);
        }
        else
        {
            stateCheckTaskDictionary[gameState] = new()
            {
                checkTask
            };
        }
    }

    public void UnregisterStateCheckTask(GameState gameState, Func<bool> checkTask)
    {
        if (stateCheckTaskDictionary.ContainsKey(gameState))
        {
            stateCheckTaskDictionary[gameState].Remove(checkTask);
        }
    }

    private bool AreAllTaskCompleted(GameState state)
    {
        if (!stateCheckTaskDictionary.TryGetValue(state, out var checkTasks) || checkTasks == null) return true;

        foreach (var task in checkTasks)
        {
            if (!task()) return false;
        }

        return true;
    }

    private IEnumerator WaitForStateTaskComplete(GameState state)
    {
        do
        {
            yield return null;
        }
        while (!AreAllTaskCompleted(state));
    }
    #endregion

    #region ChangeGameState
    private void ChangeGameState(GameState state)
    {
        if (state == currentGameState) return;

        StartCoroutine(WaitAndChangeGameState(state));
    }

    private IEnumerator WaitAndChangeGameState(GameState state)
    {
        yield return WaitForStateTaskComplete(currentGameState);

        currentGameState = state;
        UnityEngine.Debug.Log($"Game State Changed: {currentGameState}");
        OnGameStateChanged?.Invoke(currentGameState);
    }
    #endregion
}
