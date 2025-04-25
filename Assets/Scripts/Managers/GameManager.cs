using System;
using System.Collections;
using UnityEngine;

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
        PlayerDiceManager.Instance.OnFirstDiceGenerated += OnFirstDiceGenerated;
        RoundManager.Instance.OnRoundCleared += OnRoundCleared;
        RoundManager.Instance.OnRoundFailed += OnRoundFailed;
        RoundClearManager.Instance.OnRoundClearEnded += OnRoundClearEnded;
        ShopManager.Instance.OnShopEnded += OnShopEnded;
        GameResultUI.Instance.OnInfinityModeButtonClicked += OnInfinityModeButtonClicked;
        OptionUI.Instance.RegisterOnOptionValueChanged(OptionType.GameSpeed, OnGameSpeedChanged);
    }

    private void OnRoundClearEnded()
    {
        ChangeGameStateOneFrameLater(GameState.Shop);
    }

    private void OnFirstDiceGenerated()
    {
        ChangeGameStateOneFrameLater(GameState.Round);
    }

    private void OnRoundCleared(int currentRound)
    {
        if (currentRound == RoundManager.Instance.ClearRound)
        {
            ChangeGameStateOneFrameLater(GameState.GameClear);
        }
        else
        {
            ChangeGameStateOneFrameLater(GameState.RoundClear);
        }
    }

    private void OnRoundFailed(int currentRound)
    {
        ChangeGameStateOneFrameLater(GameState.GameOver);
    }

    private void OnShopEnded()
    {
        ChangeGameStateOneFrameLater(GameState.Round);
    }

    private void OnInfinityModeButtonClicked()
    {
        ChangeGameStateOneFrameLater(GameState.RoundClear);
    }

    private void OnGameSpeedChanged(int value)
    {
        Time.timeScale = 1f + (value * 0.25f);
    }
    #endregion

    private void ChangeGameStateOneFrameLater(GameState state)
    {
        SequenceManager.Instance.AddCoroutineOneFrameLater(() => CurrentGameState = state);
    }

    private IEnumerator StartGame()
    {
        yield return null;

        CurrentGameState = GameState.Loading;
    }
}