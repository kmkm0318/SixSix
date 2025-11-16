using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffectShrinkingScorePairSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectShrinkingScorePairSO")]
public class AbilityEffectShrinkingScorePairSO : AbilityEffectSO
{
    [SerializeField] private ScorePair scorePair;
    [SerializeField] private ScorePair shrinkValue;

    public override void TriggerEffect(AbilityDiceContext context)
    {
        var abilityDice = context.currentAbilityDice;
        var diceValue = abilityDice.DiceValue;
        var effectScorePair = GetScorePair(abilityDice.EffectValue);

        TriggerManager.Instance.ApplyTriggerEffect(abilityDice.transform, Vector3.down, effectScorePair);

        abilityDice.IncreaseEffectValue(diceValue);
        effectScorePair = GetScorePair(abilityDice.EffectValue);
        CheckThenRemove(effectScorePair, abilityDice);
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO, int effectValue = 0)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for AbilityEffectShrinkingScorePairSO.");
            return string.Empty;
        }

        var effectScorePair = GetScorePair(effectValue);
        var calcDescription = DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType);
        return effectDescription.GetLocalizedString(effectScorePair, shrinkValue, calcDescription);
    }

    private ScorePair GetScorePair(int effectValue)
    {
        var shrinkingValue = DiceEffectCalculator.GetCalculatedEffectValue(shrinkValue, effectValue, calculateType);
        return new ScorePair(scorePair.baseScore - shrinkingValue.baseScore, scorePair.multiplier - shrinkingValue.multiplier);
    }

    private void CheckThenRemove(ScorePair effectScorePair, AbilityDice dice)
    {
        float ellipson = 0.001f;

        if (effectScorePair.baseScore < ellipson && effectScorePair.multiplier < 1f + ellipson)
        {
            SequenceManager.Instance.AddCoroutine(() =>
            {
                DiceManager.Instance.RemoveAbilityDice(dice);
            });
        }
    }
}