using System;
using UnityEngine;

public class AvailityDice : Dice
{
    private AvailityDiceSO availityDiceSO;
    public AvailityDiceSO AvailityDiceSO => availityDiceSO;

    override protected void Start()
    {
        base.Start();
        RegisterEvents();
    }
    #region Register Events

    private void RegisterEvents()
    {
        DiceInteraction.OnIsMouseOverChanged += OnIsMouseOverChanged;
        RollManager.Instance.OnRollCompleted += OnRollCompleted;
    }

    private void OnIsMouseOverChanged(bool isMouseOver)
    {
        if (isMouseOver)
        {
            AvailityDiceToolTipUI.Instance.ShowToolTip(this);
        }
        else
        {
            AvailityDiceToolTipUI.Instance.HideToolTip();
        }
    }

    private void OnRollCompleted()
    {
        if (PlayerDiceManager.Instance.IsAvailityDiceAutoKeep)
        {
            if (FaceIndex == availityDiceSO.maxFaceValue - 1)
            {
                SetIsKeeped(true);
            }
        }
    }
    #endregion

    public void Init(AvailityDiceSO availityDiceSO, Playboard playboard)
    {
        base.Init(availityDiceSO.maxFaceValue, availityDiceSO.diceFaceSpriteListSO, playboard);

        this.availityDiceSO = availityDiceSO;
    }

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
}
