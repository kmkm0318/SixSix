using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailityTriggerDiceSO", menuName = "Scriptable Objects/AvailityTriggers/AvailityTriggerDiceSO")]
public class AvailityTriggerDiceSO : AvailityTriggerSO
{
    [SerializeField] private List<int> targetValues;
    public List<int> TargetValues => targetValues;

    public override bool IsTriggered(AvailityDiceContext context)
    {
        return context.playDice != null && targetValues.Contains(context.playDice.FaceValue);
    }

    public override string GetTriggerDescription(AvailityDiceSO availityDiceSO)
    {
        return $"When Dice is\n({string.Join(", ", targetValues)})";
    }
}