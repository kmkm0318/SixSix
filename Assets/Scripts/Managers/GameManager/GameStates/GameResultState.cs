using UnityEngine;

public class GameResultState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();

        bool isClear = GameResultManager.Instance.IsClear;
        GameResultUI.Instance.ShowGameResult(isClear);
    }

    public override void Exit()
    {
        base.Exit();
    }
}