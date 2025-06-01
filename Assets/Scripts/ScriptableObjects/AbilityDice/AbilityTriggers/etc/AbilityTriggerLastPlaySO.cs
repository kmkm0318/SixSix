using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerLastPlaySO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerLastPlaySO")]
public class AbilityTriggerLastPlaySO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        return PlayManager.Instance.PlayRemain == 1;
    }
}