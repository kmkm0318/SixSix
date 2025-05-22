using System.Collections.Generic;

public class GameStateMachine : IStateMachine<GameState>
{
    private GameState currentGameState;
    private IState currentState;
    private Dictionary<GameState, IState> gameStates;
    private Dictionary<GameState, IState> innerStates;

    public GameState CurrentGameState => currentGameState;

    public GameStateMachine()
    {
        gameStates = new()
        {
            { GameState.Loading, new LoadingState() },
            { GameState.Round, new RoundState() },
            { GameState.RoundClear, new RoundClearState() },
            { GameState.Shop, new ShopState() },
            { GameState.GameResult, new GameResultState() },
        };

        innerStates = new()
        {
            { GameState.Play, new PlayState() },
            { GameState.Roll, new RollState() },
            { GameState.Enhance, new EnhanceState() },
        };
    }

    public void ChangeState(GameState newState)
    {
        if (currentGameState == newState) return;

        if (gameStates.TryGetValue(newState, out var state))
        {
            currentGameState = newState;
            ChangeState(state);
        }
        else if (innerStates.TryGetValue(newState, out var innerState))
        {
            EnterInnerState(innerState);
        }
    }

    private void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void ExitInnerState(GameState state)
    {
        if (innerStates.TryGetValue(state, out var innerState))
        {
            innerState?.Exit();
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Attempting to exit a non-inner state: {state}");
        }
    }

    private void EnterInnerState(IState newState)
    {
        newState?.Enter();
    }

    public IState GetState(GameState state)
    {
        if (gameStates.TryGetValue(state, out var gameState))
        {
            return gameState;
        }

        if (innerStates.TryGetValue(state, out var innerState))
        {
            return innerState;
        }

        return null;
    }
}

public enum GameState
{
    None,
    Loading,
    Round,
    Play,
    Roll,
    RoundClear,
    Shop,
    Enhance,
    GameResult,
}