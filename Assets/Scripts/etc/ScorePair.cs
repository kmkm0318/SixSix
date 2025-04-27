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
}