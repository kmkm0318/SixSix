using UnityEngine;

public class GameResultState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();

        bool isClear = GameResultManager.Instance.IsClear;
        GameResultUIEvents.TriggerOnGameResultUIShowRequested(isClear);
        PlayerRecordManager.Instance.UpdatePlayerRecord();
    }

    public override void Exit()
    {
        base.Exit();
    }
}