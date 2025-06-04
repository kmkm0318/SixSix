using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerSameHandSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerSameHandSO")]
public class AbilityTriggerSameHandSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        return HandManager.Instance.IsSameHand;
    }
}