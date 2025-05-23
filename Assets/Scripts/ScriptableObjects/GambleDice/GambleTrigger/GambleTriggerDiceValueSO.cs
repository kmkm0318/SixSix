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
}