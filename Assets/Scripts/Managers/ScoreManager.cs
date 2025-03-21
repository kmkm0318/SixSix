using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public event Action<Dictionary<HandCategory, ScorePair>> OnHandCategoryScoreUpdated;
    private Dictionary<HandCategory, ScorePair> handCategoryScoreDictionary = new();

    private void Start()
    {
        RollManager.Instance.OnRollCompleted += OnRollCompleted;
    }

    private void OnRollCompleted()
    {
        var playDiceValues = PlayerDiceManager.Instance.GetPlayDiceValues();
        UpdateHandCategoryScoreDictionary(playDiceValues);
        OnHandCategoryScoreUpdated?.Invoke(handCategoryScoreDictionary);

        foreach (var pair in handCategoryScoreDictionary)
        {
            UnityEngine.Debug.Log($"{pair.Key}: {pair.Value.baseScore} * {pair.Value.multiplier}");
        }
    }

    private void UpdateHandCategoryScoreDictionary(List<int> diceValues)
    {
        if (diceValues == null || diceValues.Count == 0) return;

        ResetHandCategoryScore();
        var countMap = GetCountMap(diceValues);

        UpdateNumberCategoryScore(countMap);
        UpdateFourOfAKindCategoryScore(diceValues, countMap);
        UpdateFullHouseCategoryScore(diceValues, countMap);
        UpdateDoubleThreeOfAKindCategoryScore(diceValues, countMap);
        UpdateStraightCategoryScore(countMap);
        UpdateYachtCategoryScore(countMap);
        UpdateSixSixCategoryScore(countMap);
    }

    private Dictionary<int, int> GetCountMap(List<int> diceValues)
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

    private void ResetHandCategoryScore()
    {
        handCategoryScoreDictionary.Clear();
        foreach (var handCategory in DataContainer.Instance.HandCategoryListSO.handCategoryList)
        {
            handCategoryScoreDictionary[handCategory.handCategory] = new ScorePair(0, 0);
        }
    }

    private void UpdateNumberCategoryScore(Dictionary<int, int> countMap)
    {
        var numberCategoryList = DataContainer.Instance.NumberHandCategoryListSO.handCategoryList;
        for (int i = 0; i < numberCategoryList.Count; i++)
        {
            HandCategory handCategory = numberCategoryList[i].handCategory;
            ScorePair scorePair = numberCategoryList[i].scorePair;

            int numCount = countMap.GetValueOrDefault(i + 1, 0);
            if (numCount == 0)
            {
                scorePair = new ScorePair(0, 0);
            }
            else
            {
                scorePair.baseScore = (i + 1) * numCount;
            }

            handCategoryScoreDictionary[handCategory] = scorePair;
        }
    }

    private void UpdateFourOfAKindCategoryScore(List<int> diceValues, Dictionary<int, int> countMap)
    {
        if (countMap.Any(x => x.Value >= 4))
        {
            handCategoryScoreDictionary[HandCategory.FourOfAKind] = new(diceValues.Sum(), DataContainer.Instance.GetHandCategorySO(HandCategory.FourOfAKind).scorePair.multiplier);
        }
    }

    private void UpdateFullHouseCategoryScore(List<int> diceValues, Dictionary<int, int> countMap)
    {
        var hasThreeOrMore = countMap.Any(x => x.Value >= 3);
        var hasAnotherTwoOrMore = countMap.Count(x => x.Value >= 2) >= 2;
        if (hasThreeOrMore && hasAnotherTwoOrMore)
        {
            handCategoryScoreDictionary[HandCategory.FullHouse] = new(diceValues.Sum(), DataContainer.Instance.GetHandCategorySO(HandCategory.FullHouse).scorePair.multiplier);
        }
    }

    private void UpdateDoubleThreeOfAKindCategoryScore(List<int> diceValues, Dictionary<int, int> countMap)
    {
        var threeOrMoreCount = countMap.Count(x => x.Value >= 3);
        if (threeOrMoreCount >= 2)
        {
            handCategoryScoreDictionary[HandCategory.DoubleThreeOfAKind] = new(diceValues.Sum() * 2, DataContainer.Instance.GetHandCategorySO(HandCategory.DoubleThreeOfAKind).scorePair.multiplier);
        }
    }

    private void UpdateStraightCategoryScore(Dictionary<int, int> countMap)
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
            handCategoryScoreDictionary[HandCategory.SmallStraight] = DataContainer.Instance.GetHandCategorySO(HandCategory.SmallStraight).scorePair;
        }
        else if (maxStraightCount >= 5)
        {
            handCategoryScoreDictionary[HandCategory.LargeStraight] = DataContainer.Instance.GetHandCategorySO(HandCategory.LargeStraight).scorePair;
        }
        else if (maxStraightCount >= 6)
        {
            handCategoryScoreDictionary[HandCategory.FullStraight] = DataContainer.Instance.GetHandCategorySO(HandCategory.FullStraight).scorePair;
        }
    }

    private void UpdateYachtCategoryScore(Dictionary<int, int> countMap)
    {
        if (countMap.Any(x => x.Value >= 5))
        {
            handCategoryScoreDictionary[HandCategory.Yacht] = DataContainer.Instance.GetHandCategorySO(HandCategory.Yacht).scorePair;
        }
    }

    private void UpdateSixSixCategoryScore(Dictionary<int, int> countMap)
    {
        var maxPair = countMap.OrderByDescending(x => x.Value).FirstOrDefault();
        if (maxPair.Value >= 6)
        {
            ScorePair scorePair = DataContainer.Instance.GetHandCategorySO(HandCategory.SixSix).scorePair;
            scorePair.baseScore = maxPair.Key * 111;
            handCategoryScoreDictionary[HandCategory.Sixes] = scorePair;
        }
    }
}
