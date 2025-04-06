using System.Collections.Generic;

public class PlayDice : Dice
{
    public List<(ScorePair, int)> GetScorePairDiceIndexList()
    {
        List<(ScorePair, int)> scorePairList = new();
        ScorePair defaultScorePair = new(0, 0)
        {
            baseScore = FaceIndex + 1
        };
        scorePairList.Add((defaultScorePair, -1));
        return scorePairList;
    }
}
