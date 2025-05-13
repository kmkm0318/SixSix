using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailityTriggerDiceSO", menuName = "Scriptable Objects/AvailityTriggers/AvailityTriggerDiceSO")]
public class AvailityTriggerDiceSO : AvailityTriggerSO
{
    [SerializeField] private List<int> targetValues;
    public List<int> TargetValues => targetValues;

    public override bool IsTriggered(EffectTriggerType triggerType, AvailityDiceContext context)
    {
        return triggerType == TriggerType && context.playDice != null && targetValues.Contains(context.playDice.DiceValue);
    }

    public override string GetTriggerDescription(AvailityDiceSO availityDiceSO)
    {
        if (targetValues == null || targetValues.Count == 0)
        {
            return "No Target Values Set";
        }
        else if (targetValues.Count == 1)
        {
            return $"When Dice Value is \"{targetValues[0]}\"";
        }
        else
        {
            return $"When Dice Value is\nin ({string.Join(", ", targetValues)})";
        }
    }
}