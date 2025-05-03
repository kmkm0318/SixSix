using System;
using System.Collections.Generic;
using System.Linq;

public static class HandCalculator
{
    public static Dictionary<Hand, ScorePair> CalculateHandScore(List<int> diceValues)
    {
        if (diceValues == null || diceValues.Count == 0) return new();

        var handScoreDictionary = CreateInitialHandScoreDictionary();
        var countMap = GetCountMap(diceValues);

        UpdateChoiceScore(handScoreDictionary);
        UpdateFourOfAKindScore(handScoreDictionary, countMap);
        UpdateFullHouseScore(handScoreDictionary, countMap);
        UpdateDoubleThreeOfAKindScore(handScoreDictionary, countMap);
        UpdateStraightScore(handScoreDictionary, countMap);
        UpdateYachtScore(handScoreDictionary, countMap);
        UpdateSixSixScore(handScoreDictionary, countMap);

        return handScoreDictionary;
    }

    private static Dictionary<Hand, ScorePair> CreateInitialHandScoreDictionary()
    {
        var dictionary = new Dictionary<Hand, ScorePair>();
        foreach (var hand in DataContainer.Instance.TotalHandListSO.handList)
        {
            dictionary[hand.hand] = new(0, 0);
        }
        return dictionary;
    }

    private static Dictionary<int, int> GetCountMap(List<int> diceValues)
    {
        Dictionary<int, int> countMap = new();
        foreach (var diceValue in diceValues)
        {
            if (countMap.ContainsKey(diceValue))
            {
                countMap[diceValue]++;
            }
            else
            {
                countMap[diceValue] = 1;
            }
        }
        return countMap;
    }

    private static void UpdateChoiceScore(Dictionary<Hand, ScorePair> handScoreDictionary)
    {
        handScoreDictionary[Hand.Choice] = DataContainer.Instance.GetHandSO(Hand.Choice).scorePair;
    }

    private static void UpdateFourOfAKindScore(Dictionary<Hand, ScorePair> handScoreDictionary, Dictionary<int, int> countMap)
    {
        if (countMap.Any(x => x.Value >= 4))
        {
            handScoreDictionary[Hand.FourOfAKind] = DataContainer.Instance.GetHandSO(Hand.FourOfAKind).scorePair;
        }
    }

    private static void UpdateFullHouseScore(Dictionary<Hand, ScorePair> handScoreDictionary, Dictionary<int, int> countMap)
    {
        var hasThreeOrMore = countMap.Any(x => x.Value >= 3);
        var hasAnotherTwoOrMore = countMap.Count(x => x.Value >= 2) >= 2;
        if (hasThreeOrMore && hasAnotherTwoOrMore)
        {
            handScoreDictionary[Hand.FullHouse] = DataContainer.Instance.GetHandSO(Hand.FullHouse).scorePair;
        }
    }

    private static void UpdateDoubleThreeOfAKindScore(Dictionary<Hand, ScorePair> handScoreDictionary, Dictionary<int, int> countMap)
    {
        var threeOrMoreCount = countMap.Count(x => x.Value >= 3);
        if (threeOrMoreCount >= 2)
        {
            handScoreDictionary[Hand.DoubleThreeOfAKind] = DataContainer.Instance.GetHandSO(Hand.DoubleThreeOfAKind).scorePair;
        }
    }

    private static void UpdateStraightScore(Dictionary<Hand, ScorePair> handScoreDictionary, Dictionary<int, int> countMap)
    {
        int straightCount = 0;
        int maxStraightCount = 0;
        for (int i = 1; i <= 6; i++)
        {
            if (countMap.ContainsKey(i))
            {
                straightCount++;
                maxStraightCount = Math.Max(maxStraightCount, straightCount);
            }
            else
            {
                straightCount = 0;
            }
        }

        if (maxStraightCount >= 4)
        {
            handScoreDictionary[Hand.SmallStraight] = DataContainer.Instance.GetHandSO(Hand.SmallStraight).scorePair;
        }

        if (maxStraightCount >= 5)
        {
            handScoreDictionary[Hand.LargeStraight] = DataContainer.Instance.GetHandSO(Hand.LargeStraight).scorePair;
        }

        if (maxStraightCount >= 6)
        {
            handScoreDictionary[Hand.FullStraight] = DataContainer.Instance.GetHandSO(Hand.FullStraight).scorePair;
        }
    }

    private static void UpdateYachtScore(Dictionary<Hand, ScorePair> handScoreDictionary, Dictionary<int, int> countMap)
    {
        if (countMap.Any(x => x.Value >= 5))
        {
            handScoreDictionary[Hand.Yacht] = DataContainer.Instance.GetHandSO(Hand.Yacht).scorePair;
        }
    }

    private static void UpdateSixSixScore(Dictionary<Hand, ScorePair> handScoreDictionary, Dictionary<int, int> countMap)
    {
        var maxPair = countMap.OrderByDescending(x => x.Value).FirstOrDefault();
        if (maxPair.Value >= 6)
        {
            handScoreDictionary[Hand.SixSix] = DataContainer.Instance.GetHandSO(Hand.SixSix).scorePair;
        }
    }
}