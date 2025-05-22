using System;
using System.Collections.Generic;

[Serializable]
public struct ScorePair
{
    public double baseScore;
    public double multiplier;

    public ScorePair(double baseScore, double multiplier)
    {
        this.baseScore = baseScore;
        this.multiplier = multiplier;
    }

    public override readonly string ToString()
    {
        List<string> parts = new();
        if (baseScore != 0)
        {
            parts.Add("<color={0}>" + $"{baseScore:+0;-0;0}" + "</color>");
        }

        if (multiplier != 0 && multiplier != 1)
        {
            parts.Add("<color={1}>" + $"x{multiplier:0.##}" + "</color>");
        }

        if (parts.Count == 0)
        {
            parts.Add("<color=#888888>0</color>");
        }

        return $"Score({string.Join(", ", parts)})";
    }
}