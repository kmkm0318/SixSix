using UnityEngine;

public abstract class AvailityEffectSO : ScriptableObject
{
    [SerializeField] protected AvailityEffectCalculateType calculateType;
    public abstract void ApplyEffect(AvailityDiceContext context);
    public abstract string GetEffectDescription(AvailityDiceSO availityDiceSO);

    protected virtual int CalculateEffectValue(int value, int faceValue)
    {
        return calculateType switch
        {
            AvailityEffectCalculateType.Multiply => value * faceValue,
            AvailityEffectCalculateType.Power => Mathf.RoundToInt(Mathf.Pow(value, faceValue)),
            _ => value,
        };
    }

    protected virtual float GetCalculatedEffectValue(float value, int faceValue)
    {
        return calculateType switch
        {
            AvailityEffectCalculateType.Multiply => value * faceValue,
            AvailityEffectCalculateType.Power => Mathf.Pow(value, faceValue),
            _ => value,
        };
    }

    protected virtual string GetCalculateDescription(int maxFaceValue)
    {
        return calculateType switch
        {
            AvailityEffectCalculateType.Multiply => $"x(1~{maxFaceValue})",
            AvailityEffectCalculateType.Power => $"^(1~{maxFaceValue})",
            _ => "",
        };
    }
}

public enum AvailityEffectCalculateType
{
    Multiply,
    Power,
}