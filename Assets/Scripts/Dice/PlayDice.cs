using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        foreach (var face in Faces)
        {
            ScorePair randomScorePair = new(Random.Range(0, 11), Random.Range(0, 0));
            face.Enhance(randomScorePair);
        }

        ChangeFace(0);
    }
}
