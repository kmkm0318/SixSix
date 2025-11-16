using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffectRetriggerGambleDiceSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectRetriggerGambleDiceSO")]
public class AbilityEffectRetriggerGambleDiceSO : AbilityEffectSO
{
    public override void TriggerEffect(AbilityDiceContext context)
    {
        if (context == null || context.currentAbilityDice == null || context.gambleDice == null) return;

        int retriggerCount = context.currentAbilityDice.DiceValue;
        for (int i = 0; i < retriggerCount; i++)
        {
            RetriggerGambleDice(context);
        }
    }

    private void RetriggerGambleDice(AbilityDiceContext context)
    {
        TriggerAnimationManager.Instance.PlayRetriggerAnimation(context.currentAbilityDice.transform, Vector3.down, "green");
        SequenceManager.Instance.ApplyParallelCoroutine();
        TriggerManager.Instance.TriggerGambleDice(context.gambleDice, true);
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