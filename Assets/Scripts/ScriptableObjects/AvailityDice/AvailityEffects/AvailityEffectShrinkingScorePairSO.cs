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
        return $"Get {scorePair} And\nShrink {shrinkValue}" + DiceEffectCalculator.GetCalculateDescription(availityDiceSO.MaxDiceValue, calculateType);
    }

    private void ShrinkScorePair(int diceValue)
    {
        var shrinkingValue = DiceEffectCalculator.GetCalculatedEffectValue(shrinkValue, diceValue, calculateType);
        scorePair = new ScorePair(scorePair.baseScore - shrinkingValue.baseScore, scorePair.multiplier - shrinkingValue.multiplier);
    }

    private void CheckThenRemove(AvailityDice dice)
    {
        float ellipson = 0.001f;

        if (scorePair.baseScore < ellipson && scorePair.multiplier < 1f + ellipson)
        {
            SequenceManager.Instance.AddCoroutine(() =>
            {
                DiceManager.Instance.RemoveAvailityDice(dice);
            });
        }
    }
}