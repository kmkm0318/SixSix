using System;
using System.Collections.Generic;

[Serializable]
public struct ScorePair
{
    public float baseScore;
    public float multiplier;

    public ScorePair(float baseScore, float multiplier)
    {
        this.baseScore = baseScore;
        this.multiplier = multiplier;
    }

    public override readonly string ToString()
    {
        List<string> parts = new();
        if (baseScore != 0)
        {
            parts.Add($"{baseScore:+0;-0;0}");
        }

        if (multiplier != 0 && multiplier != 1)
        {
            parts.Add($"x{multiplier:0.##}");
        }

        if (parts.Count == 0)
        {
            parts.Add("0");
        }

        return $"Score({string.Join(", ", parts)})";
    }
}