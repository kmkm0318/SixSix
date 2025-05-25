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
            return "<color=#888888>(0)</color>";
        }

        if (hasBaseScore)
        {
            parts.Add($"<color={{0}}>{baseScore:+0;-0;0}</color>");
        }

        if (hasMultiplier)
        {
            parts.Add($"<color={{1}}>x{multiplier:0.##}</color>");
        }

        string res = string.Join(", ", parts);

        if (hasBaseScore && !hasMultiplier)
        {
            return "<color={0}>(" + res + ")</color>";
        }
        else if (!hasBaseScore && hasMultiplier)
        {
            return "<color={1}>(" + res + ")</color>";
        }
        else
        {
            return "<color={0}>(</color>" + res + "<color={1}>)</color>";
        }
    }
}