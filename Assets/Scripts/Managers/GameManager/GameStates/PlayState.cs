public class PlayState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
        RollManager.Instance.ResetRollRemain();
    }

    public override void Exit()
    {
        PlayManager.Instance.EndPlay();

        base.Exit();
        PlayManager.Instance.HandlePlayResult();
    }
}