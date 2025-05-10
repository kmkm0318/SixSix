using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectShrinkingScorePairSO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectShrinkingScorePairSO")]
public class AvailityEffectShrinkingScorePairSO : AvailityEffectSO
{
    [SerializeField] private ScorePair scorePair;
    [SerializeField] private ScorePair shrinkValue;

    public override void TriggerEffect(AvailityDiceContext context)
    {
        TriggerManager.Instance.ApplyTriggerEffect(context.availtiyDice.transform, Vector3.down, scorePair);

        ShrinkScorePair(context.availtiyDice.DiceValue);

        CheckThenRemove(context.availtiyDice);
    }

    public override string GetEffectDescription(AvailityDiceSO availityDiceSO)
    {
        return $"Get {scorePair} And Shrink\n{shrinkValue}" + GetCalculateDescription(availityDiceSO.MaxDiceValue);
    }

    private void ShrinkScorePair(int diceValue)
    {
        scorePair = new ScorePair(scorePair.baseScore - GetCalculatedEffectValue(shrinkValue.baseScore, diceValue), scorePair.multiplier - GetCalculatedEffectValue(shrinkValue.multiplier, diceValue));
    }

    private void CheckThenRemove(AvailityDice dice)
    {
        if (scorePair.baseScore <= 0 && scorePair.multiplier <= 1)
        {
            PlayerDiceManager.Instance.RemoveAvailityDice(dice);
        }
    }
}