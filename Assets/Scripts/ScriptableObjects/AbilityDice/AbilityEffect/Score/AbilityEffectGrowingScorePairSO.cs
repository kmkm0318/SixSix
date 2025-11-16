using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffectGrowingScorePairSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectGrowingScorePairSO")]
public class AbilityEffectGrowingScorePairSO : AbilityEffectSO
{
    [SerializeField] private ScorePair scorePair = new(0, 1);
    [SerializeField] private ScorePair growValue = new(0, 0);

    public override void TriggerEffect(AbilityDiceContext context)
    {
        var abilityDice = context.currentAbilityDice;
        var diceValue = abilityDice.DiceValue;

        abilityDice.IncreaseEffectValue(diceValue);

        var effectValue = abilityDice.EffectValue;
        var effectScorePair = GetScorePair(effectValue);

        TriggerManager.Instance.ApplyTriggerEffect(context.currentAbilityDice.transform, Vector3.down, effectScorePair);
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO, int effectValue = 0)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for AbilityEffectGrowingScorePairSO.");
            return string.Empty;
        }

        var effectScorePair = GetScorePair(effectValue);
        var calcDescription = DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType);
        return effectDescription.GetLocalizedString(effectScorePair, growValue, calcDescription);
    }

    private ScorePair GetScorePair(int effectValue)
    {
        var growingValue = DiceEffectCalculator.GetCalculatedEffectValue(growValue, effectValue, calculateType);
        return new ScorePair(scorePair.baseScore + growingValue.baseScore, scorePair.multiplier + growingValue.multiplier);
    }
}