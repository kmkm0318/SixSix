using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HandCategorySO", menuName = "Scriptable Objects/HandCategorySO")]
public class HandCategorySO : ScriptableObject
{
    public HandCategory handCategory;
    public string handCategoryName;
    public ScorePair scorePair;
    public int enhanceAmount;
    public int purchasePrice;

    public string GetDescriptionText()
    {
        string description = $"Enhance {handCategoryName}\nScore({scorePair.baseScore}, {scorePair.multiplier})";
        return description;
    }

    public ScorePair GetEnhancedScorePair(int enhanceLevel)
    {
        return new ScorePair(scorePair.baseScore + enhanceLevel * enhanceAmount, scorePair.multiplier + enhanceLevel * enhanceAmount);
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