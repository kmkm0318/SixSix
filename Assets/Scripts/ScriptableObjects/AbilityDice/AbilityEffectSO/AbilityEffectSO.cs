using UnityEngine;
using UnityEngine.Localization;

public abstract class AbilityEffectSO : ScriptableObject
{
    [SerializeField] protected EffectCalculateType calculateType;
    [SerializeField] protected LocalizedString effectDescription;
    public abstract void TriggerEffect(AbilityDiceContext context);
    public virtual string GetEffectDescription(AbilityDiceSO abilityDiceSO)
    {
        return effectDescription.GetLocalizedString();
    }

    protected virtual float GetCalculatedEffectValue(float value, int diceValue)
    {
        return calculateType switch
        {
            EffectCalculateType.Multiply => value * diceValue,
            EffectCalculateType.Power => Mathf.Pow(value, diceValue),
            _ => value,
        };
    }
}