using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerGambleDiceTriggeredSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerGambleDiceTriggeredSO")]
public class AbilityTriggerGambleDiceTriggeredSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;
        if (context == null || context.gambleDice == null || context.isRetriggered) return false;

        return true;
    }
}