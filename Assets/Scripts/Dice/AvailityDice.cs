using UnityEngine;

public class AvailityDice : Dice
{
    private AvailityDiceSO availityDiceSO;
    public AvailityDiceSO AvailityDiceSO => availityDiceSO;

    public void Init(AvailityDiceSO availityDiceSO, Playboard playboard)
    {
        base.Init(availityDiceSO.maxFaceValue, availityDiceSO.diceFaceSpriteListSO, playboard);

        this.availityDiceSO = availityDiceSO;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UnregisterEvents();
    }
    #region Events

    override public void HandleMouseOver(bool isMouseOver)
    {
        base.HandleMouseOver(isMouseOver);

        if (isMouseOver)
        {
            AvailityDiceToolTipUI.Instance.ShowToolTip(this);
        }
        else
        {
            AvailityDiceToolTipUI.Instance.HideToolTip();
        }
    }

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
            default:
                break;
        }
    }

    private void ApplyScorePairEffect()
    {
        ScorePair scorePair = new(availityDiceSO.scorePairAmount.baseScore, availityDiceSO.scorePairAmount.multiplier);
        scorePair.baseScore = CalculateEffectValue(scorePair.baseScore);
        scorePair.multiplier = CalculateEffectValue(scorePair.multiplier);
        ScoreManager.Instance.ApplyScorePairAndPlayDiceAnimation(this, scorePair, true);
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

    private int CalculateEffectValue(int value)
    {
        return availityDiceSO.availityDiceValueCalculationType switch
        {
            AvailityDiceValueCalculationType.Multiply => value * (FaceIndex + 1),
            AvailityDiceValueCalculationType.Power => Mathf.FloorToInt(Mathf.Pow(value, FaceIndex + 1)),
            _ => value,
        };
    }

    protected override void OnShopStarted()
    {
        base.OnShopStarted();

        IsInteractable = true;
    }

    protected override void OnShopEnded()
    {
        base.OnShopEnded();

        IsInteractable = false;
    }
}
