using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerFirstRollSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerFirstRollSO")]
public class AbilityTriggerFirstRollSO : AbilityTriggerSO
{
    int rollCount = 0;

    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        switch (triggerType)
        {
            case EffectTriggerType.PlayStarted:
                rollCount = 0;
                break;
            case EffectTriggerType.RollStarted:
                rollCount++;
                break;
        }

        if (triggerType != TriggerType) return false;

        return rollCount == 1;
    }
}