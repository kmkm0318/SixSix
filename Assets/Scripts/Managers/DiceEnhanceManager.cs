using System;

public class DiceEnhanceManager : Singleton<DiceEnhanceManager>
{
    public event Action OnEnhanceStarted;
    public event Action OnEnhanceCompleted;

    public ScorePair ScorePair { get; private set; }

    private void Start()
    {
        ShopManager.Instance.OnPlayDiceEnhancePurchaseAttempted += OnPlayDiceEnhancePurchaseAttempted;
    }

    private void OnPlayDiceEnhancePurchaseAttempted(ScorePair pair, int price, PurchaseResult result)
    {
        if (result == PurchaseResult.Success)
        {
            ScorePair = pair;
            StartEnhance();
        }
    }

    private void StartEnhance()
    {
        PlayerDiceManager.Instance.OnPlayDiceClicked += OnPlayDiceClicked;
        OnEnhanceStarted?.Invoke();
    }

    private void OnPlayDiceClicked(PlayDice dice)
    {
        dice.EnhanceDice(ScorePair);
        SequenceManager.Instance.ApplyParallelCoroutine();
        CompleteEnhance();
    }

    private void CompleteEnhance()
    {
        OnEnhanceCompleted?.Invoke();
        PlayerDiceManager.Instance.OnPlayDiceClicked -= OnPlayDiceClicked;
    }
}
