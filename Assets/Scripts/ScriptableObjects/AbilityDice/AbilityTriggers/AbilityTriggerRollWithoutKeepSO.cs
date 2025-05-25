using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerRollWithoutKeepSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerRollWithoutKeepSO")]
public class AbilityTriggerRollWithoutKeepSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        foreach (var dice in DiceManager.Instance.AllDiceList)
        {
            if (dice.IsKeeped) return false;
        }

        return true;
    }
}