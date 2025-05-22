public class PlayState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
        RollManager.Instance.ResetRollRemain();
    }

    public override void Exit()
    {
        base.Exit();
        PlayManager.Instance.EndPlay();
    }
}