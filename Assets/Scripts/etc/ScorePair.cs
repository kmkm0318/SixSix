using System;

[Serializable]
public struct ScorePair
{
    public float baseScore;
    public float multiplier;

    public ScorePair(float baseScore = 0, float multiplier = 0)
    {
        this.baseScore = baseScore;
        this.multiplier = multiplier;
    }

    public override readonly string ToString()
    {
        string res = "Score";

        if (baseScore != 0 && multiplier != 0)
        {
            res += $"(+{baseScore}, x{multiplier})";
        }
        else if (baseScore != 0)
        {
            res += $"(+{baseScore})";
        }
        else if (multiplier != 0)
        {
            res += $"(x{multiplier})";
        }
        else
        {
            res += "(0)";
        }

        return res;
    }
}