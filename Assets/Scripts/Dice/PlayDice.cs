using System.Collections.Generic;

public class PlayDice : Dice
{
    public List<ScorePair> GetScorePairList()
    {
        List<ScorePair> scorePairList = new();
        ScorePair defaultScorePair = new(0, 0)
        {
            baseScore = FaceIndex + 1
        };
        scorePairList.Add(defaultScorePair);
        return scorePairList;
    }
}
