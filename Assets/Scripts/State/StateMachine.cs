using System;
using System.Collections.Generic;

public class StateMachine<T>
{
    private T target;

    public event Action<IState<T>> OnStateChanged;
    public event Action<IState<T>> OnStateExit;

    private readonly Dictionary<GameState, IState<T>> stateDictionary = new();

    private IState<T> currentState;
    private IState<T> previousState;

    public IState<T> CurrentState => currentState;
    public IState<T> PreviousState => previousState;

    public StateMachine(T target)
    {
        this.target = target;
    }

    public void AddState(GameState state, IState<T> stateInstance)
    {
        if (stateDictionary.ContainsKey(state))
        {
            throw new ArgumentException($"State {state} already exists in the state machine.");
        }

        stateDictionary[state] = stateInstance;
    }

    public void ChangeState(GameState state)
    {
        if (!stateDictionary.TryGetValue(state, out var newState))
        {
            throw new ArgumentException($"State {state} not found in the state machine.");
        }

        if (currentState == newState)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.Exit();
            OnStateExit?.Invoke(currentState);
        }

        previousState = currentState;
        currentState = newState;
        currentState.Enter(target);
        OnStateChanged?.Invoke(currentState);
    }
}