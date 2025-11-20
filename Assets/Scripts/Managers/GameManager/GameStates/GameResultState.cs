public class GameResultState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();

        bool isClear = GameResultManager.Instance.IsClear;

        GameResultUIEvents.TriggerOnGameResultUIShowRequested(isClear);

        PlayerRecordManager.Instance.UpdatePlayerRecord();

        double highestRoundScore = ScoreManager.Instance.HighestRoundScore;
        AddScoreToLeaderboard(highestRoundScore);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private async void AddScoreToLeaderboard(double score)
    {
        await FirebaseManager.Instance.AddScoreToLeaderboard(score);
    }
}