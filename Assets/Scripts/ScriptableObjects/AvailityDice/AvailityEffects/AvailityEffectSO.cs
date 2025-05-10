using UnityEngine;

public abstract class AvailityEffectSO : ScriptableObject
{
    [SerializeField] protected AvailityEffectCalculateType calculateType;
    public abstract void TriggerEffect(AvailityDiceContext context);
    public abstract string GetEffectDescription(AvailityDiceSO availityDiceSO);

    protected virtual ScorePair GetCalculatedEffectValue(ScorePair value, int diceValue)
    {
        ScorePair result = new()
        {
            baseScore = GetCalculatedEffectValue(value.baseScore, diceValue),
            multiplier = GetCalculatedEffectValue(value.multiplier, diceValue)
        };

        return result;
    }

    protected virtual int GetCalculatedEffectValue(int value, int diceValue)
    {
        return calculateType switch
        {
            AvailityEffectCalculateType.Multiply => value * diceValue,
            AvailityEffectCalculateType.Power => Mathf.RoundToInt(Mathf.Pow(value, diceValue)),
            _ => value,
        };
    }

    protected virtual float GetCalculatedEffectValue(float value, int diceValue)
    {
        return calculateType switch
        {
            AvailityEffectCalculateType.Multiply => value * diceValue,
            AvailityEffectCalculateType.Power => Mathf.Pow(value, diceValue),
            _ => value,
        };
    }

    protected virtual string GetCalculateDescription(int maxDiceValue)
    {
        return calculateType switch
        {
            AvailityEffectCalculateType.Multiply => $"x(1~{maxDiceValue})",
            AvailityEffectCalculateType.Power => $"^(1~{maxDiceValue})",
            _ => "",
        };
    }
}

public enum AvailityEffectCalculateType
{
    Multiply,
    Power,
}