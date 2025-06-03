public class RollState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        HandManager.Instance.UpdateHands();
        TriggerManager.Instance.TriggerOnRollCompleted();
        SequenceManager.Instance.AddCoroutine(base.Exit);
    }
}