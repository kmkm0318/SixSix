using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerRollWithoutKeepPlayDiceSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerRollWithoutKeepPlayDiceSO")]
public class AbilityTriggerRollWithoutKeepPlayDiceSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        foreach (var dice in DiceManager.Instance.PlayDiceList)
        {
            if (dice.IsKeeped) return false;
        }

        Debug.Log("Triggered");

        return true;
    }
}