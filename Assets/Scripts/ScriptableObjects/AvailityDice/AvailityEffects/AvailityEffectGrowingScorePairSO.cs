using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectGrowingScorePairSO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectGrowingScorePairSO")]
public class AvailityEffectGrowingScorePairSO : AvailityEffectSO
{
    [SerializeField] private ScorePair scorePair = new(0, 1);
    [SerializeField] private ScorePair growValue = new(0, 0);

    public override void TriggerEffect(AvailityDiceContext context)
    {
        GrowScorePair(context.availtiyDice.DiceValue);

        TriggerManager.Instance.ApplyTriggerEffect(context.availtiyDice.transform, Vector3.down, scorePair);
    }

    public override string GetEffectDescription(AvailityDiceSO availityDiceSO)
    {
        return $"Get {scorePair} And Grow\n{growValue}" + GetCalculateDescription(availityDiceSO.MaxDiceValue);
    }

    private void GrowScorePair(int diceValue)
    {
        scorePair = new ScorePair(scorePair.baseScore + GetCalculatedEffectValue(growValue.baseScore, diceValue), scorePair.multiplier + GetCalculatedEffectValue(growValue.multiplier, diceValue));
    }
}