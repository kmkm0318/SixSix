using System;

public class EnhanceState : IState
{
    public event Action OnStateEnter;
    public event Action OnStateExit;

    public void Enter()
    {
        OnStateEnter?.Invoke();
    }

    public void Exit()
    {
        OnStateExit?.Invoke();
    }
}