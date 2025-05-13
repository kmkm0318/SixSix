using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HandCalculator
{
    #region GetHandCheckResults
    public static Dictionary<Hand, bool> GetHandCheckResults(List<int> diceValues)
    {
        Dictionary<Hand, bool> res = new();
        foreach (var hand in DataContainer.Instance.TotalHandListSO.handList)
        {
            res[hand.hand] = false;
        }

        if (diceValues == null || diceValues.Count == 0) return res;

        var countMap = GetCountMap(diceValues);

        res[Hand.Choice] = IsChoiceHand(countMap);
        res[Hand.FourOfAKind] = IsFourOfAKindHand(countMap);
        res[Hand.FullHouse] = IsFullHouseHand(countMap);
        res[Hand.DoubleThreeOfAKind] = IsDoubleThreeOfAKindHand(countMap);
        res[Hand.SmallStraight] = IsSmallStraightHand(countMap);
        res[Hand.LargeStraight] = IsLargeStraightHand(countMap);
        res[Hand.FullStraight] = IsFullStraightHand(countMap);
        res[Hand.Yacht] = IsYachtHand(countMap);
        res[Hand.SixSix] = IsSixSixHand(countMap);

        return res;
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

    private static bool IsChoiceHand(Dictionary<int, int> countMap)
    {
        return true;
    }

    private static bool IsFourOfAKindHand(Dictionary<int, int> countMap)
    {
        return countMap.Any(x => x.Value >= 4);
    }

    private static bool IsFullHouseHand(Dictionary<int, int> countMap)
    {
        bool hasThreeOrMore = countMap.Any(x => x.Value >= 3);
        bool hasAnotherTwoOrMore = countMap.Count(x => x.Value >= 2) >= 2;
        return hasThreeOrMore && hasAnotherTwoOrMore;
    }

    private static bool IsDoubleThreeOfAKindHand(Dictionary<int, int> countMap)
    {
        int threeOrMoreCount = countMap.Count(x => x.Value >= 3);
        return threeOrMoreCount >= 2;
    }

    private static bool IsSmallStraightHand(Dictionary<int, int> countMap)
    {
        return GetMaxStraightLength(countMap) >= 4;
    }

    private static bool IsLargeStraightHand(Dictionary<int, int> countMap)
    {
        return GetMaxStraightLength(countMap) >= 5;
    }

    private static bool IsFullStraightHand(Dictionary<int, int> countMap)
    {
        return GetMaxStraightLength(countMap) >= 6;
    }

    private static int GetMaxStraightLength(Dictionary<int, int> countMap)
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
        return maxStraightCount;
    }

    private static bool IsYachtHand(Dictionary<int, int> countMap)
    {
        return countMap.Any(x => x.Value >= 5);
    }

    private static bool IsSixSixHand(Dictionary<int, int> countMap)
    {
        return countMap.Any(x => x.Value >= 6);
    }

    #endregion

    public static Dictionary<Hand, ScorePair> GetHandScorePairs(List<int> diceValues)
    {
        if (diceValues == null || diceValues.Count == 0) return new();

        var handScoreDictionary = new Dictionary<Hand, ScorePair>();
        foreach (var hand in DataContainer.Instance.TotalHandListSO.handList)
        {
            handScoreDictionary[hand.hand] = new(0, 0);
        }

        var handCheckResults = GetHandCheckResults(diceValues);

        foreach (var hand in DataContainer.Instance.TotalHandListSO.handList)
        {
            if (handCheckResults[hand.hand])
            {
                handScoreDictionary[hand.hand] = DataContainer.Instance.GetHandSO(hand.hand).scorePair;
            }
        }

        return handScoreDictionary;
    }

    #region GetHandProbabilities
    public static Dictionary<Hand, float> GetHandProbabilities(List<int> diceValues)
    {
        Dictionary<Hand, float> res = new();
        foreach (var hand in DataContainer.Instance.TotalHandListSO.handList)
        {
            res[hand.hand] = 0;
        }

        if (diceValues == null || diceValues.Count == 0) return res;

        int maxRollDiceNum = Mathf.CeilToInt(diceValues.Count / 2f);

        return CalculateHandProbabilities(diceValues, maxRollDiceNum);
    }

    private static Dictionary<Hand, float> CalculateHandProbabilities(List<int> diceValues, int maxRollDiceNum)
    {
        Dictionary<Hand, float> res = new();

        foreach (var hand in DataContainer.Instance.TotalHandListSO.handList)
        {
            res[hand.hand] = 0;
        }

        for (int i = 0; i <= maxRollDiceNum; i++)
        {
            Dictionary<Hand, float> tmp = new();

            var (handCounts, totalCount) = GetHandSuccessCounts(diceValues, i);

            foreach (var hand in handCounts.Keys)
            {
                tmp[hand] = handCounts[hand] / (float)totalCount;
            }

            foreach (var pair in tmp)
            {
                if (!res.ContainsKey(pair.Key))
                {
                    res[pair.Key] = pair.Value;
                }
                else
                {
                    res[pair.Key] = Mathf.Max(res[pair.Key], pair.Value);
                }
            }
        }

        return res;
    }

    private static (Dictionary<Hand, int>, int) GetHandSuccessCounts(List<int> diceValues, int rollNum)
    {
        Dictionary<Hand, int> res = new();

        if (rollNum == 0)
        {
            var result = GetHandCheckResults(diceValues);
            foreach (var hand in result.Keys)
            {
                if (result[hand])
                {
                    res[hand] = 1;
                }
                else
                {
                    res[hand] = 0;
                }
            }
            return (res, 1);
        }

        var allIndexCombination = GetAllIndexCombinations(diceValues.Count, rollNum);
        var allDiceCombinations = GetAllDiceCombinations(rollNum);

        foreach (var indexCombination in allIndexCombination)
        {
            Dictionary<Hand, int> tmp = new();

            foreach (var diceCombination in allDiceCombinations)
            {
                var tempDiceValues = new List<int>(diceValues);
                for (int i = 0; i < indexCombination.Count; i++)
                {
                    tempDiceValues[indexCombination[i]] = diceCombination[i];
                }

                var result = GetHandCheckResults(tempDiceValues);
                foreach (var hand in result.Keys)
                {
                    if (result[hand])
                    {
                        if (!tmp.ContainsKey(hand))
                        {
                            tmp[hand] = 0;
                        }
                        tmp[hand]++;
                    }
                }
            }

            foreach (var hand in tmp.Keys)
            {
                if (!res.ContainsKey(hand))
                {
                    res[hand] = tmp[hand];
                }
                res[hand] = Mathf.Max(res[hand], tmp[hand]);
            }
        }

        return (res, allDiceCombinations.Count);
    }

    private static List<List<int>> GetAllIndexCombinations(int totalCount, int pickCount)
    {
        List<List<int>> res = new();
        GetIndexCombinations(0, totalCount, pickCount, new(), res);
        return res;
    }

    private static void GetIndexCombinations(int start, int totalCount, int pickCount, List<int> current, List<List<int>> res)
    {
        if (pickCount == 0)
        {
            res.Add(new List<int>(current));
            return;
        }

        for (int i = start; i <= totalCount - pickCount; i++)
        {
            current.Add(i);
            GetIndexCombinations(i + 1, totalCount, pickCount - 1, current, res);
            current.RemoveAt(current.Count - 1);
        }
    }

    private static List<List<int>> GetAllDiceCombinations(int count)
    {
        List<List<int>> res = new();
        GetDiceCombination(count, new(), res);
        return res;
    }

    private static List<int> GetDiceCombination(int remain, List<int> current, List<List<int>> res)
    {
        if (remain == 0)
        {
            res.Add(new List<int>(current));
            return current;
        }

        for (int i = 1; i <= 6; i++)
        {
            current.Add(i);
            GetDiceCombination(remain - 1, current, res);
            current.RemoveAt(current.Count - 1);
        }

        return current;
    }
    #endregion
}