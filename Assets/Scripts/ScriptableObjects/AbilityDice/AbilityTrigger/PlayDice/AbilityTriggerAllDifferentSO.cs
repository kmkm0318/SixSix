using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerAllDifferentSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerAllDifferentSO")]
public class AbilityTriggerAllDifferentSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        var diceList = DiceManager.Instance.PlayDiceList;
        var used = new HashSet<int>();

        foreach (var dice in diceList)
        {
            if (used.Contains(dice.DiceValue)) return false;
            used.Add(dice.DiceValue);
        }

        return true;
    }
}