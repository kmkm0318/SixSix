using UnityEngine;

[CreateAssetMenu(fileName = "HandSO", menuName = "Scriptable Objects/HandSO")]
public class HandSO : ScriptableObject
{
    public Hand hand;
    public string handName;
    public ScorePair scorePair;
    public ScorePair enhanceAmount;

    public string GetDescriptionText()
    {
        string description = $"Enhance {handName}\nScore(+{enhanceAmount.baseScore}, x{enhanceAmount.multiplier})";
        return description;
    }

    public ScorePair GetEnhancedScorePair(int enhanceLevel)
    {
        return new ScorePair(scorePair.baseScore + enhanceLevel * enhanceAmount.baseScore, scorePair.multiplier + enhanceLevel * enhanceAmount.multiplier);
    }
}

public enum Hand
{
    Choice, FourOfAKind, FullHouse, SmallStraight, LargeStraight, Yacht,
    DoubleThreeOfAKind, FullStraight, SixSix,
}