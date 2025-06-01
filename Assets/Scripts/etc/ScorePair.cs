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
        bool hasBaseScore = baseScore != 0;
        bool hasMultiplier = multiplier != 0 && multiplier != 1;

        if (!hasBaseScore && !hasMultiplier)
        {
            return "<color=gray>(0)</color>";
        }

        if (hasBaseScore)
        {
            parts.Add($"<color=blue>{baseScore:+0;-0;0}</color>");
        }

        if (hasMultiplier)
        {
            parts.Add($"<color=red>x{multiplier:0.##}</color>");
        }

        string res = string.Join(", ", parts);

        if (hasBaseScore && !hasMultiplier)
        {
            return "<color=blue>(" + res + ")</color>";
        }
        else if (!hasBaseScore && hasMultiplier)
        {
            return "<color=red>(" + res + ")</color>";
        }
        else
        {
            return "<color=blue>(</color>" + res + "<color=red>)</color>";
        }
    }
}