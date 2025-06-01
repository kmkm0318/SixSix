using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffectShrinkingScorePairSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectShrinkingScorePairSO")]
public class AbilityEffectShrinkingScorePairSO : AbilityEffectSO
{
    [SerializeField] private ScorePair scorePair;
    [SerializeField] private ScorePair shrinkValue;

    public override void TriggerEffect(AbilityDiceContext context)
    {
        TriggerManager.Instance.ApplyTriggerEffect(context.currentAbilityDice.transform, Vector3.down, scorePair);

        ShrinkScorePair(context.currentAbilityDice.DiceValue);

        CheckThenRemove(context.currentAbilityDice);
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for AbilityEffectShrinkingScorePairSO.");
            return string.Empty;
        }
        effectDescription.Arguments = new object[] { scorePair, shrinkValue, DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType) };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }

    private void ShrinkScorePair(int diceValue)
    {
        var shrinkingValue = DiceEffectCalculator.GetCalculatedEffectValue(shrinkValue, diceValue, calculateType);
        scorePair = new ScorePair(scorePair.baseScore - shrinkingValue.baseScore, scorePair.multiplier - shrinkingValue.multiplier);
    }

    private void CheckThenRemove(AbilityDice dice)
    {
        float ellipson = 0.001f;

        if (scorePair.baseScore < ellipson && scorePair.multiplier < 1f + ellipson)
        {
            SequenceManager.Instance.AddCoroutine(() =>
            {
                DiceManager.Instance.RemoveAbilityDice(dice);
            });
        }
    }
}