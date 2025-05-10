using UnityEngine;

[CreateAssetMenu(fileName = "AvailityTriggerHandSO", menuName = "Scriptable Objects/AvailityTriggers/AvailityTriggerHandSO")]
public class AvailityTriggerHandSO : AvailityTriggerSO
{
    [SerializeField] private HandSO targetHand;
    public HandSO TargetHand => targetHand;

    public override bool IsTriggered(AvailityTriggerType triggerType, AvailityDiceContext context)
    {
        return triggerType == TriggerType && (targetHand == null || context.handSO == targetHand);
    }

    public override string GetTriggerDescription(AvailityDiceSO availityDiceSO)
    {
        if (targetHand == null)
        {
            return "When Play With Any Hand";
        }
        else
        {
            return $"When Play With\n\"{targetHand.handName}\"";
        }
    }
}