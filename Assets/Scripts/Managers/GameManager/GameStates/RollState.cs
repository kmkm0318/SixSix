public class RollState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        HandManager.Instance.UpdateHands();
        TriggerManager.Instance.TriggerGambleDices();
        SequenceManager.Instance.AddCoroutine(DiceManager.Instance.ClearGambleDices);
        base.Exit();
    }
}