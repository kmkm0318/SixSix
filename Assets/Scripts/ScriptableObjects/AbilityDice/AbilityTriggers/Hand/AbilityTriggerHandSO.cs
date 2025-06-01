using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerHandSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerHandSO")]
public class AbilityTriggerHandSO : AbilityTriggerSO
{
    [SerializeField] private HandSO targetHand;
    public HandSO TargetHand => targetHand;

    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        return triggerType == TriggerType && targetHand != null && context.handSO == targetHand;
    }

    public override string GetTriggerDescription(AbilityDiceSO abilityDiceSO)
    {
        if (triggerDescription == null)
        {
            Debug.LogError("Trigger description is not set for " + name);
            return string.Empty;
        }
        triggerDescription.Arguments = new object[] { targetHand.HandName };
        triggerDescription.RefreshString();
        return triggerDescription.GetLocalizedString();
    }
}