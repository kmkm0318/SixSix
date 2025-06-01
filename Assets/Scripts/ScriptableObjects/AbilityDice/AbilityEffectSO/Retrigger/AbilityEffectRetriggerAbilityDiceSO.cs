using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffectRetriggerAbilityDiceSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectRetriggerAbilityDiceSO")]
public class AbilityEffectRetriggerAbilityDiceSO : AbilityEffectSO
{
    public override void TriggerEffect(AbilityDiceContext context)
    {
        if (context == null || context.currentAbilityDice == null || context.abilityDice == null || context.currentAbilityDice == context.abilityDice) return;

        int retriggerCount = context.currentAbilityDice.DiceValue;
        for (int i = 0; i < retriggerCount; i++)
        {
            RetriggerAbilityDice(context);
        }
    }

    private void RetriggerAbilityDice(AbilityDiceContext context)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(context.currentAbilityDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();
        TriggerManager.Instance.TriggerAbilityDice(context.abilityDice, null, true);
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for AbilityEffectMoneySO.");
            return string.Empty;
        }
        effectDescription.Arguments = new object[] { DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType) };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }
}