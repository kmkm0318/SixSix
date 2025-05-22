public class GameResultState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();

        bool isClear = RoundManager.Instance.CurrentRound == RoundManager.Instance.ClearRound && ScoreManager.Instance.CurrentRoundScore >= ScoreManager.Instance.TargetRoundScore;
        GameResultUI.Instance.ShowGameResult(isClear);
    }

    public override void Exit()
    {
        base.Exit();
    }
}