using System;

public class LoadingState : IState
{
    public event Action OnStateEnter;
    public event Action OnStateExit;

    public void Enter()
    {
        OnStateEnter?.Invoke();
        DiceManager.Instance.StartFirstPlayDiceGenerate(RoundManager.Instance.StartNextRound);
    }

    public void Exit()
    {
        OnStateExit?.Invoke();
    }
}

