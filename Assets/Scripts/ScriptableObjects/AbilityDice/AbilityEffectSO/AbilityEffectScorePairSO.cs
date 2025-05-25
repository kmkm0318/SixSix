using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffectScorePairSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectScorePairSO")]
public class AbilityEffectScorePairSO : AbilityEffectSO
{
    [SerializeField] private ScorePair scorePair;

    public override void TriggerEffect(AbilityDiceContext context)
    {
        ScorePair resultScorePair = DiceEffectCalculator.GetCalculatedEffectValue(scorePair, context.abilityDice.DiceValue, calculateType);

        TriggerManager.Instance.ApplyTriggerEffect(context.abilityDice.transform, Vector3.down, resultScorePair);
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for AbilityEffectScorePairSO.");
            return string.Empty;
        }
        effectDescription.Arguments = new object[] { scorePair, DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType) };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }
}