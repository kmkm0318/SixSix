using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action<GameState> OnGameStateChanged;

    private GameState currentGameState = GameState.None;
    public GameState CurrentGameState
    {
        get => currentGameState;
        set
        {
            if (currentGameState == value) return;
            currentGameState = value;
            OnGameStateChanged?.Invoke(currentGameState);
        }
    }

    private void Start()
    {
        Init();
        RegisterEvents();
        StartCoroutine(StartGame());
    }

    private void Init()
    {
        OnGameSpeedChanged(OptionManager.Instance.OptionData.gameSpeed);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        OptionUI.Instance.RegisterOnOptionValueChanged(OptionType.GameSpeed, OnGameSpeedChanged);
    }

    private void OnGameSpeedChanged(int value)
    {
        Time.timeScale = 1f + (value * 0.25f);
    }
    #endregion

    private IEnumerator StartGame()
    {
        yield return null;

        CurrentGameState = GameState.Loading;
    }
}

public enum GameState
{
    None,
    Loading,
    Round,
    RoundClear,
    Shop,
    GameClear,
    GameOver,
}