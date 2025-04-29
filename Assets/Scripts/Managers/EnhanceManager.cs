using System;

public class EnhanceManager : Singleton<EnhanceManager>
{
    public event Action OnDiceEnhanceStarted;
    public event Action OnDiceEnhanceCompleted;
    public event Action OnHandEnhanceStarted;
    public event Action OnHandEnhanceCompleted;

    public ScorePair ScorePair { get; private set; }

    private void Start()
    {
        ShopManager.Instance.OnPlayDiceEnhancePurchaseAttempted += OnPlayDiceEnhancePurchaseAttempted;
        ShopManager.Instance.OnHandEnhancePurchaseAttempted += OnHandEnhancePurchaseAttempted;
    }

    #region Play Dice Enhancement
    private void OnPlayDiceEnhancePurchaseAttempted(ScorePair pair, int price, PurchaseResult result)
    {
        if (result == PurchaseResult.Success)
        {
            ScorePair = pair;
            StartDiceEnhance();
        }
    }

    private void StartDiceEnhance()
    {
        PlayerDiceManager.Instance.OnPlayDiceClicked += OnPlayDiceClicked;
        OnDiceEnhanceStarted?.Invoke();
    }

    private void OnPlayDiceClicked(PlayDice dice)
    {
        dice.EnhanceDice(ScorePair);
        SequenceManager.Instance.ApplyParallelCoroutine();
        CompleteDiceEnhance();
    }

    private void CompleteDiceEnhance()
    {
        OnDiceEnhanceCompleted?.Invoke();
        PlayerDiceManager.Instance.OnPlayDiceClicked -= OnPlayDiceClicked;
    }
    #endregion

    private void OnHandEnhancePurchaseAttempted(HandSO sO, PurchaseResult result)
    {
        if (result == PurchaseResult.Success)
        {
            StartHandEnhance();
        }
    }

    private void StartHandEnhance()
    {
        OnHandEnhanceStarted?.Invoke();
        HandScoreUI.Instance.OnHandSelected += OnHandSelected;
    }

    private void OnHandSelected(HandSO sO, ScorePair pair)
    {
        throw new NotImplementedException();
    }

    private void CompleteHandEnhance()
    {
        OnHandEnhanceCompleted?.Invoke();
    }
}
