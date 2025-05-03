using System;
using System.Xml.Serialization;

public class EnhanceManager : Singleton<EnhanceManager>
{
    public event Action OnDiceEnhanceStarted;
    public event Action OnDiceEnhanceCompleted;
    public event Action OnHandEnhanceStarted;
    public event Action OnHandEnhanceCompleted;

    public int HandEnhanceLevel { get; private set; }
    public ScorePair DiceEnhanceValue { get; private set; }

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
        PlayerDiceManager.Instance.OnPlayDiceClicked += OnPlayDiceClicked;
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
        PlayerDiceManager.Instance.OnPlayDiceClicked -= OnPlayDiceClicked;
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
        HandScoreUI.Instance.OnHandSelected += OnHandSelected;
        OnHandEnhanceStarted?.Invoke();
    }

    private void OnHandSelected(HandSO sO, ScorePair pair)
    {
        HandScoreUI.Instance.EnhanceHand(sO, HandEnhanceLevel);
        CompleteHandEnhance();
    }

    private void CompleteHandEnhance()
    {
        HandScoreUI.Instance.OnHandSelected -= OnHandSelected;
        OnHandEnhanceCompleted?.Invoke();
    }
}
