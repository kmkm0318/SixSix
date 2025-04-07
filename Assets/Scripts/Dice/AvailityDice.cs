using UnityEngine;

public class AvailityDice : Dice
{
    private AvailityDiceSO availityDiceSO;
    public AvailityDiceSO AvailityDiceSO => availityDiceSO;

    public void Init(AvailityDiceSO availityDiceSO, Playboard playboard)
    {
        this.availityDiceSO = availityDiceSO;

        Init(availityDiceSO.maxFaceValue, availityDiceSO.diceFaceSpriteListSO, playboard);
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
        ScoreManager.Instance.ApplyScorePairAndPlayDiceAnimation(this, scorePair);
    }

    public bool IsTriggeredByPlayDice(PlayDice playDice)
    {
        return availityDiceSO.targetPlayDiceValueList.Contains(playDice.FaceIndex + 1);
    }

    public bool IsTriggeredByHandCategory(HandCategorySO handCategorySO)
    {
        return availityDiceSO.targetHandCategorySO == handCategorySO;
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
