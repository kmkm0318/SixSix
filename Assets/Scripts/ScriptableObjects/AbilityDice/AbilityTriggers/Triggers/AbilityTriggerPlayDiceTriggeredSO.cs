using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerPlayDiceTriggeredSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerPlayDiceTriggeredSO")]
public class AbilityTriggerPlayDiceTriggeredSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;
        if (context == null || context.playDice == null || context.isRetriggered) return false;

        return true;
    }
}