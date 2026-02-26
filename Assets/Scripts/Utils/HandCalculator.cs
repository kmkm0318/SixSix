using System;
using System.Collections.Generic;
using UnityEngine;

public static class HandCalculator
{
    public static Dictionary<Hand, bool> HandCheckResultsCache { get; private set; } = new();
    public static Dictionary<int, int> CountMapCache { get; private set; } = new();

    #region GetHandCheckResults
    public static Dictionary<Hand, bool> GetHandCheckResultsNonAlloc(List<int> diceValues)
    {
        foreach (var hand in DataContainer.Instance.TotalHandListSO.handList)
        {
            HandCheckResultsCache[hand.hand] = false;
        }

        if (diceValues == null || diceValues.Count == 0) return HandCheckResultsCache;

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

        return HandCheckResultsCache;
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

        int twoOrMoreCount = 0;
        foreach (var pair in countMap)
        {
            if (pair.Value >= 2)
            {
                twoOrMoreCount++;
            }
        }
        return hasThreeOrMore && twoOrMoreCount >= 2;
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
            if (pair.Value >= 6)
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
            var handCounts = GetHandSuccessCounts(diceValues, i);
            int totalCount = (int)Mathf.Pow(6, i);

            foreach (var pair in handCounts)
            {
                if (!res.ContainsKey(pair.Key))
                {
                    res[pair.Key] = pair.Value / (float)totalCount;
                }
                else
                {
                    res[pair.Key] = Mathf.Max(res[pair.Key], pair.Value / (float)totalCount);
                }
            }
        }

        return res;
    }

    private static Dictionary<Hand, int> GetHandSuccessCounts(List<int> diceValues, int rollNum)
    {
        // 각 핸드의 최대 성공 횟수
        Dictionary<Hand, int> successCounts = new();

        // 현재 고르는 주사위 조합에서의 각 핸드 성공 횟수
        Dictionary<Hand, int> tmpCounts = new();

        // pickMap을 통해 고르는 주사위 조합 표현, 1이면 고름, 0이면 안고름
        for (int pickMap = 0; pickMap < (1 << diceValues.Count); pickMap++)
        {
            // pickMap에서 고르는 주사위 수를 세기
            int pickCount = 0;
            for (int i = 0; i < diceValues.Count; i++)
            {
                pickCount += (pickMap & (1 << i)) != 0 ? 1 : 0;
            }

            // pickCount가 굴리는 주사위 수와 다르면 패스
            if (pickCount != rollNum) continue;

            // 임시 횟수 초기화
            tmpCounts.Clear();

            // 탐색
            DFS(diceValues, pickMap, 0, tmpCounts);

            // 최대 성공 횟수 갱신
            foreach (var hand in tmpCounts.Keys)
            {
                if (!successCounts.ContainsKey(hand))
                {
                    successCounts[hand] = tmpCounts[hand];
                }
                else
                {
                    successCounts[hand] = Mathf.Max(successCounts[hand], tmpCounts[hand]);
                }
            }
        }

        // 최대 성공 횟수 반환
        return successCounts;
    }

    private static void DFS(List<int> diceValues, int pickMap, int idx, Dictionary<Hand, int> successCounts)
    {
        // 인덱스 초과 시 중지
        if (idx >= diceValues.Count)
        {
            // 핸드 체크
            var handCheckResults = GetHandCheckResultsNonAlloc(diceValues);

            // 성공 핸드 카운트 증가
            foreach (var hand in handCheckResults.Keys)
            {
                if (handCheckResults[hand])
                {
                    if (!successCounts.ContainsKey(hand))
                    {
                        successCounts[hand] = 0;
                    }
                    successCounts[hand]++;
                }
            }

            // 탐색 종료
            return;
        }

        // 현재 인덱스의 주사위를 고르지 않는 경우 탐색
        if ((pickMap & (1 << idx)) == 0)
        {
            DFS(diceValues, pickMap, idx + 1, successCounts);
            return;
        }

        // 현재 인덱스의 주사위를 고르는 경우 탐색

        // 값 저장
        var originalValue = diceValues[idx];

        // 1~6까지 값으로 변경 후 DFS 호출
        for (int i = 1; i <= 6; i++)
        {
            diceValues[idx] = i;
            DFS(diceValues, pickMap, idx + 1, successCounts);
        }

        // 값 복구
        diceValues[idx] = originalValue;
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