using System;
using System.Collections;
using UnityEngine;

public enum GameState
{
    None,
    Loading,
    GeneratingDice,
    PlayingHand,
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
            currentGameState = value;
            OnGameStateChanged?.Invoke(currentGameState);
        }
    }

    void Start()
    {
        Invoke(nameof(LoadingGame), 0.1f);
    }

    private IEnumerator LoadingGame()
    {
        CurrentGameState = GameState.Loading;

        yield return new WaitForSeconds(1f);

        StartCoroutine(GeneratePlayDice(5, GameState.PlayingHand));
    }

    private IEnumerator GeneratePlayDice(int diceCount, GameState nextState)
    {
        int targetDiceCount = PlayerDiceManager.Instance.PlayDiceList.Count + diceCount;
        CurrentGameState = GameState.GeneratingDice;

        while (PlayerDiceManager.Instance.PlayDiceList.Count < targetDiceCount)
        {
            yield return new WaitForSeconds(0.1f);
        }

        CurrentGameState = nextState;
    }
}
