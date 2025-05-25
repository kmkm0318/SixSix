using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GambleTriggerMaxValueSO", menuName = "Scriptable Objects/GambleTriggers/GambleTriggerMaxValueSO")]
public class GambleTriggerMaxValueSO : GambleTriggerSO
{
    public override bool IsTriggered(GambleDice gambleDice)
    {
        return gambleDice.DiceValue == gambleDice.DiceValueMax;
    }

    public override string GetTriggerDescription(GambleDiceSO gambleDiceSO)
    {
        if (triggerDescription == null)
        {
            Debug.LogError("Trigger description is not set for " + name);
            return string.Empty;
        }
        triggerDescription.Arguments = new object[] { gambleDiceSO.maxDiceValue };
        triggerDescription.RefreshString();
        return triggerDescription.GetLocalizedString();
    }
}