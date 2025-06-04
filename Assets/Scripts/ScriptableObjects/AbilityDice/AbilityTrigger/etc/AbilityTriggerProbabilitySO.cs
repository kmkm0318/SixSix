using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerProbabilitySO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerProbabilitySO")]
public class AbilityTriggerProbabilitySO : AbilityTriggerSO
{
    [SerializeField] private AbilityTriggerSO targetTriggerSO;
    [SerializeField] private int probNumerator = 1;
    [SerializeField] private int probDenominator = 6;

    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (!targetTriggerSO.IsTriggered(triggerType, context)) return false;

        int randomValue = Random.Range(1, probDenominator + 1);
        return randomValue <= probNumerator;
    }

    public override string GetTriggerDescription(AbilityDiceSO abilityDiceSO)
    {
        if (targetTriggerSO == null)
        {
            Debug.LogError("Trigger SO is not set for " + name);
            return "Error: No trigger available.";
        }

        string targetTriggerDescription = targetTriggerSO.GetTriggerDescription(abilityDiceSO);
        if (targetTriggerDescription == null)
        {
            Debug.LogError("Trigger description is not set for " + name);
            return "Error: No description available.";
        }

        string probVal = probNumerator + "/" + probDenominator;

        triggerDescription.Arguments = new object[] { probVal };
        triggerDescription.RefreshString();

        string probDescription = triggerDescription.GetLocalizedString();

        return $"{targetTriggerDescription}\n{probDescription}";
    }
}