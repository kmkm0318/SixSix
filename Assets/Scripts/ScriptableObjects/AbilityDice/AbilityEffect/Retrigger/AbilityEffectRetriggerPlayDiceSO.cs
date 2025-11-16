using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffectRetriggerPlayDiceSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectRetriggerPlayDiceSO")]
public class AbilityEffectRetriggerPlayDiceSO : AbilityEffectSO
{
    public override void TriggerEffect(AbilityDiceContext context)
    {
        if (context == null || context.currentAbilityDice == null || context.playDice == null) return;

        int retriggerCount = context.currentAbilityDice.DiceValue;
        for (int i = 0; i < retriggerCount; i++)
        {
            RetriggerPlayDice(context);
        }
    }

    private void RetriggerPlayDice(AbilityDiceContext context)
    {
        TriggerAnimationManager.Instance.PlayRetriggerAnimation(context.currentAbilityDice.transform, Vector3.down, "blue");
        SequenceManager.Instance.ApplyParallelCoroutine();
        TriggerManager.Instance.TriggerPlayDice(context.playDice, true);
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO, int effectValue = 0)
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