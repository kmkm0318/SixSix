using System;

public abstract class BaseGameState : IState
{
    public event Action OnStateEnter;
    public event Action OnStateExit;

    public virtual void Enter()
    {
        OnStateEnter?.Invoke();
    }

    public virtual void Exit()
    {
        OnStateExit?.Invoke();
    }
}