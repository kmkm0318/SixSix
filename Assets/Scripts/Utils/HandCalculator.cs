using System;
using System.Collections.Generic;
using UnityEngine;

public static class HandCalculator
{
    public static Dictionary<Hand, bool> HandCheckResultsCache { get; private set; } = new();
    public static Dictionary<int, int> CountMapCache { get; private set; } = new();

    #region GetHandCheckResults
    public static void GetHandCheckResultsNonAlloc(List<int> diceValues)
    {
        foreach (var hand in DataContainer.Instance.TotalHandListSO.handList)
        {
            HandCheckResultsCache[hand.hand] = false;
        }

        if (diceValues == null || diceValues.Count == 0) return;

        GetCountMapNonAlloc(diceValues);

        HandCheckResultsCache[Hand.Choice] = IsChoiceHand(CountMapCache);
        HandCheckResultsCache[Hand.FourOfAKind] = IsFourOfAKindHand(CountMapCache);
        HandCheckResultsCache[Hand.FullHouse] = IsFullHouseHand(CountMapCache);
        HandCheckResultsCache[Hand.DoubleThreeOfAKind] = IsDoubleThreeOfAKindHand(CountMapCache);
        HandCheckResultsCache[Hand.SmallStraight] = IsSmallStraightHand(CountMapCache);
        HandCheckResultsCache[Hand.LargeStraight] = IsLargeStraightHand(CountMapCache);
        HandCheckResultsCache[Hand.FullStraight] = IsFullStraightHand(CountMapCache);
        HandCheckResultsCache[Hand.Yacht] = IsYachtHand(CountMapCache);
        HandCheckResultsCache[Hand.SixSix] = IsSixSixHand(CountMapCache);
    }

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

    private static void GetCountMapNonAlloc(List<int> diceValues)
    {
        CountMapCache.Clear();
        foreach (var diceValue in diceValues)
        {
            if (CountMapCache.ContainsKey(diceValue))
            {
                CountMapCache[diceValue]++;
            }
            else
            {
                CountMapCache[diceValue] = 1;
            }
        }
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
        foreach (var pair in countMap)
        {
            if (pair.Value >= 4)
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsFullHouseHand(Dictionary<int, int> countMap)
    {
        bool hasThreeOrMore = false;
        foreach (var pair in countMap)
        {
            if (pair.Value >= 3)
            {
                hasThreeOrMore = true;
                break;
            }
        }

        int hasAnotherTwoOrMoreCount = 0;
        foreach (var pair in countMap)
        {
            if (pair.Value >= 2)
            {
                hasAnotherTwoOrMoreCount++;
            }
        }
        return hasThreeOrMore && hasAnotherTwoOrMoreCount >= 2;
    }

    private static bool IsDoubleThreeOfAKindHand(Dictionary<int, int> countMap)
    {
        int threeOrMoreCount = 0;
        foreach (var pair in countMap)
        {
            if (pair.Value >= 3)
            {
                threeOrMoreCount++;
            }
        }
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
        foreach (var pair in countMap)
        {
            if (pair.Value >= 5)
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsSixSixHand(Dictionary<int, int> countMap)
    {
        foreach (var pair in countMap)
        {
            if (pair.Value >= 2)
            {
                return true;
            }
        }
        return false;
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

            var handCounts = GetHandSuccessCounts(diceValues, i);
            int totalCount = (int)Mathf.Pow(6, i);

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

    private static Dictionary<Hand, int> GetHandSuccessCounts(List<int> diceValues, int rollNum)
    {
        Dictionary<Hand, int> successCounts = new();

        if (rollNum == 0)
        {
            var result = GetHandCheckResults(diceValues);
            foreach (var hand in result.Keys)
            {
                if (result[hand])
                {
                    successCounts[hand] = 1;
                }
                else
                {
                    successCounts[hand] = 0;
                }
            }
            return successCounts;
        }

        DFS(new List<int>(diceValues), rollNum, 0, successCounts);

        return successCounts;
    }

    private static void DFS(List<int> currentValues, int pickCount, int idx, Dictionary<Hand, int> successCounts)
    {
        // 인덱스 초과, 고를 수 있는 수가 0 이하이면 중지
        if (idx >= currentValues.Count || pickCount <= 0)
        {
            // 고를 수 있는 수가 0이 아니면 패스
            if (pickCount != 0) return;

            // 핸드 체크
            GetHandCheckResultsNonAlloc(currentValues);

            // 성공 핸드 카운트 증가
            foreach (var hand in HandCheckResultsCache.Keys)
            {
                if (HandCheckResultsCache[hand])
                {
                    if (!successCounts.ContainsKey(hand))
                    {
                        successCounts[hand] = 0;
                    }
                    successCounts[hand]++;
                }
            }

            return;
        }

        // 고를 수 있는 수가 남은 주사위보다 많으면 종료
        if (pickCount > currentValues.Count - idx) return;

        // 현재 인덱스의 주사위를 고르는 경우 탐색

        // 값 저장
        var originalValue = currentValues[idx];

        // 1~6까지 값으로 변경 후 DFS 호출
        for (int i = 1; i <= 6; i++)
        {
            currentValues[idx] = i;
            DFS(currentValues, pickCount - 1, idx + 1, successCounts);
        }

        // 값 복구
        currentValues[idx] = originalValue;

        // 현재 인덱스의 주사위를 고르지 않는 경우 탐색
        DFS(currentValues, pickCount, idx + 1, successCounts);
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