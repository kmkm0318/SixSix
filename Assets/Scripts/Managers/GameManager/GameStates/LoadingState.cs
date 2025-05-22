public class LoadingState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
        LoadingManager.Instance.StartLoading(() =>
        {
            DiceManager.Instance.StartFirstPlayDiceGenerate(() =>
            {
                GameManager.Instance.ChangeState(GameState.Round);
            });
        });

    }

    public override void Exit()
    {
        base.Exit();
    }
}