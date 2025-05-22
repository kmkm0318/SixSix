public class EnhanceState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
        if (EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Dice)
        {
            PlayManager.Instance.PlayRemain = 0;
        }
        else if (EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Hand)
        {
            PlayManager.Instance.PlayRemain = 1;
        }

        GameManager.Instance.ChangeState(GameState.Play);
    }

    public override void Exit()
    {
        base.Exit();
    }
}