using System;

public interface IState
{
    event Action OnStateEnter;
    event Action OnStateExit;

    void Enter();
    void Exit();
}