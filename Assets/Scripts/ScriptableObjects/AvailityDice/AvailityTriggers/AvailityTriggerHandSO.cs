using UnityEngine;

[CreateAssetMenu(fileName = "AvailityTriggerHandSO", menuName = "Scriptable Objects/AvailityTriggers/AvailityTriggerHandSO")]
public class AvailityTriggerHandSO : AvailityTriggerSO
{
    [SerializeField] private HandSO targetHand;
    public HandSO TargetHand => targetHand;

    public override bool IsTriggered(AvailityDiceContext context)
    {
        return context.handSO != null && context.handSO == targetHand;
    }

    public override string GetTriggerDescription(AvailityDiceSO availityDiceSO)
    {
        return $"When Hand is\n{targetHand.handName}";
    }
}