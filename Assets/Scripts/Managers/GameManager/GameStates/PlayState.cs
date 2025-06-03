public class PlayState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
        RollManager.Instance.StartRoll();
    }

    public override void Exit()
    {
        PlayManager.Instance.EndPlay();

        base.Exit();
        PlayManager.Instance.HandlePlayResult();
    }
}