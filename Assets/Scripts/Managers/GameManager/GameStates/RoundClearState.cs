public class RoundClearState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
        if (RoundManager.Instance.IsBossRound)
        {
            SequenceManager.Instance.AddCoroutine(() =>
            {
                GambleDiceSaveManager.Instance.TryAddRandomBossGambleDiceIcon();
            });
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}