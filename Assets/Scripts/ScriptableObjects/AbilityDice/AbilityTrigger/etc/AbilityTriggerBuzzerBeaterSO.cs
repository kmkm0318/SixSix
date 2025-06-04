using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerBuzzerBeaterSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerBuzzerBeaterSO")]
public class AbilityTriggerBuzzerBeaterSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        return PlayManager.Instance.PlayRemain == 1 && RollManager.Instance.RollRemain == 0;
    }
}