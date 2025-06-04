using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerMultipleOfNSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerMultipleOfNSO")]
public class AbilityTriggerMultipleOfNSO : AbilityTriggerSO
{
    [SerializeField] private int multipleOfN = 7;

    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        var playDiceList = DiceManager.Instance.PlayDiceList;
        int sum = 0;
        foreach (var playDice in playDiceList)
        {
            sum += playDice.DiceValue;
        }
        return sum % multipleOfN == 0;
    }

    public override string GetTriggerDescription(AbilityDiceSO abilityDiceSO)
    {
        if (triggerDescription == null)
        {
            Debug.LogError("Trigger description is not set for " + name);
            return string.Empty;
        }
        triggerDescription.Arguments = new object[] { multipleOfN };
        triggerDescription.RefreshString();
        return triggerDescription.GetLocalizedString();
    }
}