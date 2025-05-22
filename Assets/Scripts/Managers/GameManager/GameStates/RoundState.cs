public class RoundState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
        RoundManager.Instance.StartNextRound();
        if (RoundManager.Instance.IsBossRound)
        {
            BossRoundManager.Instance.EnterBossRound();
        }
        ScoreManager.Instance.UpdateTargetRoundScore();
        PlayManager.Instance.ResetPlayRemain();
        GameManager.Instance.ChangeState(GameState.Play);
    }

    public override void Exit()
    {
        if (RoundManager.Instance.IsBossRound)
        {
            BossRoundManager.Instance.ExitBossRound();
        }
        base.Exit();
    }
}