public class GambleDiceManager : Singleton<GambleDiceManager>
{
    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        DiceManager.Instance.OnGambleDiceClicked += OnGambleDiceClicked;
    }

    private void OnGambleDiceClicked(GambleDice dice)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(dice.transform);
        dice.TriggerEffect();
        SequenceManager.Instance.ApplyParallelCoroutine();
        SequenceManager.Instance.AddCoroutine(() =>
        {
            DiceManager.Instance.RemoveGambleDice(dice);
        });
    }
}