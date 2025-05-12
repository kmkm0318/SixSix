using UnityEngine;

[CreateAssetMenu(fileName = "AvailityTriggerRollWithoutKeepSO", menuName = "Scriptable Objects/AvailityTriggers/AvailityTriggerRollWithoutKeepSO")]
public class AvailityTriggerRollWithoutKeepSO : AvailityTriggerSO
{
    public override bool IsTriggered(AvailityTriggerType triggerType, AvailityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        foreach (var dice in DiceManager.Instance.AllDiceList)
        {
            if (dice.IsKeeped) return false;
        }

        return true;
    }

    public override string GetTriggerDescription(AvailityDiceSO availityDiceSO)
    {
        return "When Roll Without Keep";
    }
}