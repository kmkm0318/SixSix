using UnityEngine;

public abstract class AvailityEffectSO : ScriptableObject
{
    [SerializeField] protected EffectCalculateType calculateType;
    public abstract void TriggerEffect(AvailityDiceContext context);
    public abstract string GetEffectDescription(AvailityDiceSO availityDiceSO);

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