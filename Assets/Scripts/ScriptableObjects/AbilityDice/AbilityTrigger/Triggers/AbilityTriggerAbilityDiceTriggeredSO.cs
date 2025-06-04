using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerAbilityDiceTriggeredSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerAbilityDiceTriggeredSO")]
public class AbilityTriggerAbilityDiceTriggeredSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;
        if (context == null || context.abilityDice == null || context.currentAbilityDice == context.abilityDice || context.isRetriggered) return false;
        if (context.abilityDice.AbilityDiceSO.abilityEffect is AbilityEffectRetriggerAbilityDiceSO) return false;

        return true;
    }
}