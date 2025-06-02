using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameStateMachine gameStateManager;

    public GameState CurrentGameState => gameStateManager.CurrentGameState;

    public event Action OnGameStateChanged;

    protected override void Awake()
    {
        base.Awake();

        gameStateManager = new GameStateMachine();
    }

    private void Start()
    {
        RegisterEvents();
        ChangeState(GameState.Loading);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        RegisterEvent(GameState.Loading, () =>
        {
            Debuging("Loading State Entered");
        }, () =>
        {
            Debuging("Loading State Exited");
        });

        RegisterEvent(GameState.Round, () =>
        {
            Debuging("Round State Entered");
        }, () =>
        {
            Debuging("Round State Exited");
        });

        RegisterEvent(GameState.RoundClear, () =>
        {
            Debuging("RoundClear State Entered");
        }, () =>
        {
            Debuging("RoundClear State Exited");
        });

        RegisterEvent(GameState.Shop, () =>
        {
            Debuging("Shop State Entered");
        }, () =>
        {
            Debuging("Shop State Exited");
        });

        RegisterEvent(GameState.Play, () =>
        {
            Debuging("Play State Entered");
        }, () =>
        {
            Debuging("Play State Exited");
        });

        RegisterEvent(GameState.Roll, () =>
        {
            Debuging("Roll State Entered");
        }, () =>
        {
            Debuging("Roll State Exited");
        });

        RegisterEvent(GameState.Enhance, () =>
        {
            Debuging("Enhance State Entered");
        }, () =>
        {
            Debuging("Enhance State Exited");
        });

        RegisterEvent(GameState.GameResult, () =>
        {
            Debuging("GameResult State Entered");
        }, () =>
        {
            Debuging("GameResult State Exited");
        });
    }

    private void Debuging(string message)
    {
        Debug.Log(message);
    }
    #endregion

    #region Game State
    public void ChangeState(GameState newState)
    {
        gameStateManager.ChangeState(newState);
        OnGameStateChanged?.Invoke();
    }

    public void ExitState(GameState state)
    {
        gameStateManager.ExitInnerState(state);
        OnGameStateChanged?.Invoke();
    }

    public void RegisterEvent(GameState state, Action onEnter = null, Action onExit = null)
    {
        var targetState = gameStateManager.GetState(state);
        if (targetState == null)
        {
            Debug.LogWarning($"Game state {state} not found.");
            return;
        }

        targetState.OnStateEnter += onEnter;
        targetState.OnStateExit += onExit;
    }

    public void UnregisterEvent(GameState state, Action onEnter = null, Action onExit = null)
    {
        var targetState = gameStateManager.GetState(state);
        if (targetState == null)
        {
            Debug.LogWarning($"Game state {state} not found.");
            return;
        }

        targetState.OnStateEnter -= onEnter;
        targetState.OnStateExit -= onExit;
    }
    #endregion
}

