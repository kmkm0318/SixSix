using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameStateMachine gameStateManager;

    public GameState CurrentGameState => gameStateManager.CurrentGameState;

    protected override void Awake()
    {
        base.Awake();

        gameStateManager = new GameStateMachine();
    }

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGMType.GameScene);

        ChangeState(GameState.Loading);
    }

    #region Game State
    public void ChangeState(GameState newState)
    {
        gameStateManager.ChangeState(newState);
    }

    public void ExitState(GameState state)
    {
        gameStateManager.ExitInnerState(state);
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

