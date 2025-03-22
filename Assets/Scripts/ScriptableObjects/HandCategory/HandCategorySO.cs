using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HandCategorySO", menuName = "Scriptable Objects/HandCategorySO")]
public class HandCategorySO : ScriptableObject
{
    public HandCategory handCategory;
    public string handCategoryName;
    public ScorePair scorePair;
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

    public ScorePair(int baseScore, int multiplier)
    {
        this.baseScore = baseScore;
        this.multiplier = multiplier;
    }
}