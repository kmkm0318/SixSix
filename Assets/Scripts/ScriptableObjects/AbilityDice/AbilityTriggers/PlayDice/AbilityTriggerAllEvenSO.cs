using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerAllEvenSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerAllEvenSO")]
public class AbilityTriggerAllEvenSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        var diceList = DiceManager.Instance.PlayDiceList;

        foreach (var dice in diceList)
        {
            if (dice == null || dice.DiceValue % 2 != 0)
            {
                return false;
            }
        }

        return true;
    }
}