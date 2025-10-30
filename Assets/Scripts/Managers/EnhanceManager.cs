public class EnhanceManager : Singleton<EnhanceManager>
{
    public EnhanceType CurrentEnhanceType { get; private set; }
    public int HandEnhanceLevel { get; private set; }
    public ScorePair DiceEnhanceValue { get; private set; }

    private void Start()
    {
        ShopManager.Instance.OnEnhancePurchaseAttempted += OnEnhancePurchaseAttempted;
    }

    #region Enhancement
    private void OnEnhancePurchaseAttempted(EnhancePurchaseContext context, PurchaseResult result)
    {
        if (result != PurchaseResult.Success) return;

        if (context.EnhanceType == EnhanceType.Dice)
        {
            DiceEnhanceValue = context.EnhanceValue;
            StartDiceEnhance();
        }
        else
        {
            HandEnhanceLevel = context.EnhanceLevel;
            StartHandEnhance();
        }
    }

    private void StartDiceEnhance()
    {
        CurrentEnhanceType = EnhanceType.Dice;
        DiceManager.Instance.OnPlayDiceClicked += OnPlayDiceClicked;
        GameManager.Instance.ChangeState(GameState.Enhance);
    }

    private void OnPlayDiceClicked(PlayDice dice)
    {
        dice.EnhanceDice(DiceEnhanceValue);
        CompleteDiceEnhance();
    }

    private void CompleteDiceEnhance()
    {
        DiceManager.Instance.OnPlayDiceClicked -= OnPlayDiceClicked;
        GameManager.Instance.ExitState(GameState.Play);
        GameManager.Instance.ExitState(GameState.Enhance);
    }

    private void StartHandEnhance()
    {
        CurrentEnhanceType = EnhanceType.Hand;
        HandManager.Instance.OnEnhanceHandSelected += OnHandSelected;
        GameManager.Instance.ChangeState(GameState.Enhance);
    }

    private void OnHandSelected(HandSO sO)
    {
        HandManager.Instance.EnhanceHand(sO.hand, HandEnhanceLevel);
        CompleteHandEnhance();
    }

    private void CompleteHandEnhance()
    {
        HandManager.Instance.OnEnhanceHandSelected -= OnHandSelected;
        GameManager.Instance.ExitState(GameState.Play);
        GameManager.Instance.ExitState(GameState.Enhance);
    }
    #endregion

    #region EnhanceValues
    public int GetEnhancePrice(int enhanceLevel)
    {
        return enhanceLevel * 3 - (enhanceLevel - 1);
    }

    public ScorePair GetEnhanceValue(int enhanceLevel, bool isRandom)
    {
        int totalEnhance = enhanceLevel * 5;
        int baseScoreEnhance = isRandom ? UnityEngine.Random.Range(1, totalEnhance) : totalEnhance / 2;
        int multiplierEnhance = totalEnhance - baseScoreEnhance;

        float baseScore = baseScoreEnhance * 5f;
        float multiplier = multiplierEnhance * 0.05f;

        return new ScorePair(baseScore, multiplier);
    }
    #endregion
}

public enum EnhanceType
{
    Dice,
    Hand
}