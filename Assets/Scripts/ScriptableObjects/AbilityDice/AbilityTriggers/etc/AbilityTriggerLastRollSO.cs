using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerLastRollSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerLastRollSO")]
public class AbilityTriggerLastRollSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        return RollManager.Instance.RollRemain == 0;
    }
}