using System;

public class RoundState : IState
{
    public event Action OnStateEnter;
    public event Action OnStateExit;

    public void Enter()
    {
        OnStateEnter?.Invoke();
        ScoreManager.Instance.UpdateTargetRoundScore();
        PlayManager.Instance.StartPlay(true);
    }

    public void Exit()
    {
        OnStateExit?.Invoke();
    }
}
