using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerAllSmallSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerAllSmallSO")]
public class AbilityTriggerAllSmallSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        var diceList = DiceManager.Instance.PlayDiceList;

        foreach (var dice in diceList)
        {
            if (dice == null || dice.DiceValue >= 4)
            {
                return false;
            }
        }

        return true;
    }
}