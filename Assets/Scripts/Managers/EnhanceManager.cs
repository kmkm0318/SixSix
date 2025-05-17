using System;

public class EnhanceManager : Singleton<EnhanceManager>
{
    public EnhanceType CurrentEnhanceType { get; private set; }
    public int HandEnhanceLevel { get; private set; }
    public ScorePair DiceEnhanceValue { get; private set; }

    public event Action OnDiceEnhanceStarted;
    public event Action OnDiceEnhanceCompleted;
    public event Action OnHandEnhanceStarted;
    public event Action OnHandEnhanceCompleted;

    private void Start()
    {
        ShopManager.Instance.OnPlayDiceEnhancePurchaseAttempted += OnPlayDiceEnhancePurchaseAttempted;
        ShopManager.Instance.OnHandEnhancePurchaseAttempted += OnHandEnhancePurchaseAttempted;
    }

    #region Play Dice Enhancement
    private void OnPlayDiceEnhancePurchaseAttempted(DiceEnhancePurchaseContext context, PurchaseResult result)
    {
        if (result == PurchaseResult.Success)
        {
            DiceEnhanceValue = context.EnhanceValue;
            StartDiceEnhance();
        }
    }

    private void StartDiceEnhance()
    {
        CurrentEnhanceType = EnhanceType.Dice;
        DiceManager.Instance.OnPlayDiceClicked += OnPlayDiceClicked;
        OnDiceEnhanceStarted?.Invoke();
    }

    private void OnPlayDiceClicked(PlayDice dice)
    {
        dice.EnhanceDice(DiceEnhanceValue);
        CompleteDiceEnhance();
    }

    private void CompleteDiceEnhance()
    {
        OnDiceEnhanceCompleted?.Invoke();
        DiceManager.Instance.OnPlayDiceClicked -= OnPlayDiceClicked;
    }
    #endregion

    private void OnHandEnhancePurchaseAttempted(HandEnhancePurchaseContext context, PurchaseResult result)
    {
        if (result == PurchaseResult.Success)
        {
            HandEnhanceLevel = context.EnhanceLevel;
            StartHandEnhance();
        }
    }

    private void StartHandEnhance()
    {
        CurrentEnhanceType = EnhanceType.Hand;
        HandManager.Instance.OnHandSelected += OnHandSelected;
        OnHandEnhanceStarted?.Invoke();
    }

    private void OnHandSelected(HandSO sO)
    {
        HandManager.Instance.EnhanceHand(sO, HandEnhanceLevel);
        CompleteHandEnhance();
    }

    private void CompleteHandEnhance()
    {
        HandManager.Instance.OnHandSelected -= OnHandSelected;
        OnHandEnhanceCompleted?.Invoke();
    }
}

public enum EnhanceType
{
    Dice,
    Hand
}