using UnityEngine;

public class AvailityDice : Dice
{
    private AvailityDiceSO availityDiceSO;
    public AvailityDiceSO AvailityDiceSO => availityDiceSO;
    public int SellPrice => availityDiceSO.sellPrice;

    public void Init(AvailityDiceSO availityDiceSO, Playboard playboard)
    {
        base.Init(availityDiceSO.maxFaceValue, availityDiceSO.diceFaceSpriteListSO, playboard);

        this.availityDiceSO = availityDiceSO;
    }

    #region Events
    override protected void OnRollCompleted()
    {
        base.OnRollCompleted();

        if (PlayerDiceManager.Instance.IsAvailityDiceAutoKeep)
        {
            if (FaceIndex == availityDiceSO.maxFaceValue - 1)
            {
                IsKeeped = true;
            }
        }
    }
    #endregion

    public void ApplyEffect()
    {
        if (availityDiceSO == null) return;

        switch (availityDiceSO.availityEffectType)
        {
            case AvailityEffectType.ApplyScorePair:
                ApplyScorePairEffect();
                break;
            case AvailityEffectType.AchieveMoney:
                ApplyAchieveMoneyEffect();
                break;
            default:
                break;
        }
    }

    private void ApplyScorePairEffect()
    {
        ScorePair scorePair = new(availityDiceSO.scorePairAmount.baseScore, availityDiceSO.scorePairAmount.multiplier);
        scorePair.baseScore = CalculateEffectValue(scorePair.baseScore);
        scorePair.multiplier = CalculateEffectValue(scorePair.multiplier);

        for (int i = 0; i < GetPowerValue(); i++)
        {
            ScoreManager.Instance.ApplyScorePairAndPlayDiceAnimation(this, scorePair, true);
        }
    }

    private void ApplyAchieveMoneyEffect()
    {
        int money = availityDiceSO.moneyAmount;
        money = CalculateEffectValue(money);

        for (int i = 0; i < GetPowerValue(); i++)
        {
            ScoreManager.Instance.ApplyMoneyAndPlayDiceAnimation(this, money, true);
        }
    }

    private float CalculateEffectValue(float value)
    {
        return availityDiceSO.availityDiceValueCalculationType switch
        {
            AvailityDiceValueCalculationType.Multiply => value * (FaceIndex + 1),
            AvailityDiceValueCalculationType.Power => Mathf.FloorToInt(Mathf.Pow(value, FaceIndex + 1)),
            _ => value,
        };
    }

    private int CalculateEffectValue(int value)
    {
        return (int)CalculateEffectValue((float)value);
    }

    private int GetPowerValue()
    {
        return availityDiceSO.availityDiceValueCalculationType == AvailityDiceValueCalculationType.Power ? FaceIndex + 1 : 1;
    }

    public bool IsTriggeredByPlayDice(PlayDice playDice)
    {
        if (availityDiceSO.availityTriggerType == AvailityTriggerType.OnHandCategoryApplied) return false;
        return availityDiceSO.targetPlayDiceValueList.Contains(playDice.FaceIndex + 1);
    }

    public bool IsTriggeredByHandCategory(HandCategorySO handCategorySO)
    {
        if (availityDiceSO.availityTriggerType == AvailityTriggerType.OnPlayDiceApplied) return false;
        return availityDiceSO.targetHandCategorySO == null || availityDiceSO.targetHandCategorySO == handCategorySO;
    }

    protected override void OnShopStarted()
    {
        IsInteractable = true;
    }

    protected override void OnShopEnded()
    {
        IsInteractable = false;
    }

    public override DiceHighlightType GetHighlightType()
    {
        var type = base.GetHighlightType();

        if (type != DiceHighlightType.None) return type;

        if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            return DiceHighlightType.Sell;
        }
        else
        {
            return DiceHighlightType.None;
        }
    }

    public override void ShowToolTip()
    {
        string name = availityDiceSO.diceName;
        string description = availityDiceSO.GetDescriptionText();
        ToolTipUI.Instance.ShowToolTip(this, transform, Vector3.down, name, description);
    }
}
