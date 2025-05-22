using System;
using UnityEngine;

public static class DiceEffectCalculator
{
    public static ScorePair GetCalculatedEffectValue(ScorePair value, int diceValue, EffectCalculateType calculateType)
    {
        ScorePair result = new()
        {
            baseScore = GetCalculatedEffectValue(value.baseScore, diceValue, calculateType),
            multiplier = GetCalculatedEffectValue(value.multiplier, diceValue, calculateType)
        };

        return result;
    }

    public static int GetCalculatedEffectValue(int value, int diceValue, EffectCalculateType calculateType)
    {
        return calculateType switch
        {
            EffectCalculateType.Multiply => value * diceValue,
            EffectCalculateType.Power => Mathf.RoundToInt(Mathf.Pow(value, diceValue)),
            _ => diceValue,
        };
    }

    public static double GetCalculatedEffectValue(double value, int diceValue, EffectCalculateType calculateType)
    {
        return calculateType switch
        {
            EffectCalculateType.Multiply => value * diceValue,
            EffectCalculateType.Power => Math.Pow(value, diceValue),
            _ => diceValue,
        };
    }

    public static string GetCalculateDescription(int maxDiceValue, EffectCalculateType calculateType)
    {
        string range = maxDiceValue > 1 ? $"1~{maxDiceValue}" : "1";
        range = $"(<color={{2}}>{range}</color>)";

        return calculateType switch
        {
            EffectCalculateType.Multiply => $"x{range}",
            EffectCalculateType.Power => $"^{range}",
            _ => range
        };
    }
}

public enum EffectCalculateType
{
    None,
    Multiply,
    Power,
}