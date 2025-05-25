using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffectGrowingScorePairSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectGrowingScorePairSO")]
public class AbilityEffectGrowingScorePairSO : AbilityEffectSO
{
    [SerializeField] private ScorePair scorePair = new(0, 1);
    [SerializeField] private ScorePair growValue = new(0, 0);

    public override void TriggerEffect(AbilityDiceContext context)
    {
        GrowScorePair(context.abilityDice.DiceValue);

        TriggerManager.Instance.ApplyTriggerEffect(context.abilityDice.transform, Vector3.down, scorePair);
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for AbilityEffectGrowingScorePairSO.");
            return string.Empty;
        }
        effectDescription.Arguments = new object[] { scorePair, growValue, DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType) };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }

    private void GrowScorePair(int diceValue)
    {
        var growingValue = DiceEffectCalculator.GetCalculatedEffectValue(growValue, diceValue, calculateType);
        scorePair = new ScorePair(scorePair.baseScore + growingValue.baseScore, scorePair.multiplier + growingValue.multiplier);
    }
}