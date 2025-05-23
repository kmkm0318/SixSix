public class RoundClearState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
        if (RoundManager.Instance.IsBossRound)
        {
            GambleDiceSaveManager.Instance.TryAddRandomBossGambleDiceIcon();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}