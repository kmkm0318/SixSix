using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerDiceSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerDiceSO")]
public class AbilityTriggerDiceSO : AbilityTriggerSO
{
    [SerializeField] private List<int> targetValues;
    public List<int> TargetValues => targetValues;

    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        return triggerType == TriggerType && context.playDice != null && targetValues.Contains(context.playDice.DiceValue);
    }

    public override string GetTriggerDescription(AbilityDiceSO abilityDiceSO)
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