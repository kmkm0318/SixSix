using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HandCategorySO", menuName = "Scriptable Objects/HandCategorySO")]
public class HandCategorySO : ScriptableObject
{
    public HandCategory handCategory;
    public string handCategoryName;
    public ScorePair scorePair;
    public ScorePair enhanceAmount;
    public int purchasePrice;

    public string GetDescriptionText()
    {
        string description = $"Enhance {handCategoryName}\nScore(+{enhanceAmount.baseScore}, x{enhanceAmount.multiplier})";
        return description;
    }

    public ScorePair GetEnhancedScorePair(int enhanceLevel)
    {
        return new ScorePair(scorePair.baseScore + enhanceLevel * enhanceAmount.baseScore, scorePair.multiplier + enhanceLevel * enhanceAmount.multiplier);
    }
}

public enum HandCategory
{
    Choice, FourOfAKind, FullHouse, SmallStraight, LargeStraight, Yacht,
    DoubleThreeOfAKind, FullStraight, SixSix,
}

[Serializable]
public struct ScorePair
{
    public int baseScore;
    public int multiplier;

    public ScorePair(int baseScore = 0, int multiplier = 0)
    {
        this.baseScore = baseScore;
        this.multiplier = multiplier;
    }
}