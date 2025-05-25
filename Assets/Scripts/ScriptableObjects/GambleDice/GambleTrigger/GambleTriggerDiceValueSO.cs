using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GambleTriggerDiceValueSO", menuName = "Scriptable Objects/GambleTriggers/GambleTriggerDiceValueSO")]
public class GambleTriggerDiceValueSO : GambleTriggerSO
{
    [SerializeField] private List<int> targetValues;

    public override bool IsTriggered(GambleDice gambleDice)
    {
        return targetValues.Contains(gambleDice.DiceValue);
    }

    public override string GetTriggerDescription(GambleDiceSO gambleDiceSO)
    {
        if (triggerDescription == null)
        {
            Debug.LogError("Trigger description is not set for " + name);
            return string.Empty;
        }
        triggerDescription.Arguments = new object[] { string.Join(", ", targetValues) };
        triggerDescription.RefreshString();
        return triggerDescription.GetLocalizedString();
    }
}