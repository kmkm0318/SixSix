using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerFirstPlaySO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerFirstPlaySO")]
public class AbilityTriggerFirstPlaySO : AbilityTriggerSO
{
    int playCount = 0;

    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        switch (triggerType)
        {
            case EffectTriggerType.RoundStarted:
                playCount = 0;
                break;
            case EffectTriggerType.PlayStarted:
                playCount++;
                break;
        }

        if (triggerType != TriggerType) return false;

        return playCount == 1;
    }
}