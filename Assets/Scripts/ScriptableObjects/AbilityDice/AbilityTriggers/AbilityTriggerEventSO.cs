using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerEventSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerEventSO")]
public class AbilityTriggerEventSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        return triggerType == TriggerType;
    }
}