using System;

public class PlayState : IState
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
        PlayManager.Instance.HandlePlayResult();
    }
}
