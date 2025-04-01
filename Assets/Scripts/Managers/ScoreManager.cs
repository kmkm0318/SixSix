using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public event Action<Dictionary<HandCategory, ScorePair>> OnHandCategoryScoreUpdated;
    private Dictionary<HandCategory, ScorePair> handCategoryScoreDictionary = new();

    public event Action<int> OnTargetRoundScoreUpdated;
    public event Action<int> OnCurrentRoundScoreUpdated;
    public event Action<int> TargetRoundScoreUpdated;
    public event Action<int> CurrentRoundScoreUpdated;
    public event Action<int> PlayScoreUpdated;
    public event Action<ScorePair> ScorePairUpdated;
    public event Action<int> BaseScoreUpdated;
    public event Action<int> MultiplierUpdated;

    private int targetRoundScore = 0;
    public int TargetRoundScore
    {
        get => targetRoundScore;
        private set
        {
            if (targetRoundScore == value) return;
            targetRoundScore = value;
            TargetRoundScoreUpdated?.Invoke(targetRoundScore);
        }
    }

    private int currentRoundScore = 0;
    public int CurrentRoundScore
    {
        get => currentRoundScore;
        private set
        {
            if (currentRoundScore == value) return;
            currentRoundScore = value;
            CurrentRoundScoreUpdated?.Invoke(currentRoundScore);
        }
    }

    private int playScore = 0;
    public int PlayScore
    {
        get => playScore;
        private set
        {
            if (playScore == value) return;
            playScore = value;
            PlayScoreUpdated?.Invoke(playScore);
        }
    }

    private ScorePair scorePair = new(0, 0);
    public ScorePair ScorePair
    {
        get => scorePair;
        private set
        {
            if (scorePair.Equals(value)) return;
            scorePair = value;
            ScorePairUpdated?.Invoke(scorePair);
        }
    }

    private int baseScore = 0;
    public int BaseScore
    {
        get => baseScore;
        private set
        {
            if (baseScore == value) return;
            baseScore = value;
            BaseScoreUpdated?.Invoke(baseScore);
        }
    }

    private int multiplier = 0;
    public int Multiplier
    {
        get => multiplier;
        private set
        {
            if (multiplier == value) return;
            multiplier = value;
            MultiplierUpdated?.Invoke(multiplier);
        }
    }

    private void Start()
    {
        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        RoundManager.Instance.OnRoundStarted += OnRoundStarted;
        RoundManager.Instance.OnRoundCleared += OnRoundCleared;
        RollManager.Instance.OnRollCompleted += OnRollCompleted;
        HandCategoryScoreUI.Instance.OnHandCategorySelected += OnHandCategorySelected;
    }

    private void OnRoundStarted(int currentRound)
    {
        UpdateTargetRoundScore(currentRound);
    }

    private void OnRoundCleared(int currentRound)
    {
        UpdateCurrentRoundScore(0);
    }

    private void OnRollCompleted()
    {
        var playDiceValues = PlayerDiceManager.Instance.GetPlayDiceValues();
        UpdateHandCategoryScoreDictionary(playDiceValues);
        OnHandCategoryScoreUpdated?.Invoke(handCategoryScoreDictionary);
    }

    private void OnHandCategorySelected(ScorePair pair)
    {
        ScorePair = pair;
        PlayScore = ScorePair.baseScore * ScorePair.multiplier;
        ScorePair = new();

        if (CheckAddOverFlow(CurrentRoundScore, PlayScore))
        {
            UpdateCurrentRoundScore(int.MaxValue);
        }
        else
        {
            UpdateCurrentRoundScore(CurrentRoundScore + PlayScore);
        }
    }
    #endregion

    #region CalculateHandCategoryScore
    private void UpdateHandCategoryScoreDictionary(List<int> diceValues)
    {
        if (diceValues == null || diceValues.Count == 0) return;

        ResetHandCategoryScore();
        var countMap = GetCountMap(diceValues);

        UpdateChoiceScore(countMap);
        UpdateFourOfAKindScore(diceValues, countMap);
        UpdateFullHouseScore(diceValues, countMap);
        UpdateDoubleThreeOfAKindScore(diceValues, countMap);
        UpdateStraightScore(countMap);
        UpdateYachtScore(countMap);
        UpdateSixSixScore(countMap);
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

    private void UpdateChoiceScore(Dictionary<int, int> countMap)
    {
        handCategoryScoreDictionary[HandCategory.Choice] = new(countMap.Sum(x => x.Key * x.Value), DataContainer.Instance.GetHandCategorySO(HandCategory.Choice).scorePair.multiplier);
    }

    private void UpdateFourOfAKindScore(List<int> diceValues, Dictionary<int, int> countMap)
    {
        if (countMap.Any(x => x.Value >= 4))
        {
            handCategoryScoreDictionary[HandCategory.FourOfAKind] = new(diceValues.Sum(), DataContainer.Instance.GetHandCategorySO(HandCategory.FourOfAKind).scorePair.multiplier);
        }
    }

    private void UpdateFullHouseScore(List<int> diceValues, Dictionary<int, int> countMap)
    {
        var hasThreeOrMore = countMap.Any(x => x.Value >= 3);
        var hasAnotherTwoOrMore = countMap.Count(x => x.Value >= 2) >= 2;
        if (hasThreeOrMore && hasAnotherTwoOrMore)
        {
            handCategoryScoreDictionary[HandCategory.FullHouse] = new(diceValues.Sum(), DataContainer.Instance.GetHandCategorySO(HandCategory.FullHouse).scorePair.multiplier);
        }
    }

    private void UpdateDoubleThreeOfAKindScore(List<int> diceValues, Dictionary<int, int> countMap)
    {
        var threeOrMoreCount = countMap.Count(x => x.Value >= 3);
        if (threeOrMoreCount >= 2)
        {
            handCategoryScoreDictionary[HandCategory.DoubleThreeOfAKind] = new(diceValues.Sum() * 2, DataContainer.Instance.GetHandCategorySO(HandCategory.DoubleThreeOfAKind).scorePair.multiplier);
        }
    }

    private void UpdateStraightScore(Dictionary<int, int> countMap)
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

        if (maxStraightCount >= 5)
        {
            handCategoryScoreDictionary[HandCategory.LargeStraight] = DataContainer.Instance.GetHandCategorySO(HandCategory.LargeStraight).scorePair;
        }

        if (maxStraightCount >= 6)
        {
            handCategoryScoreDictionary[HandCategory.FullStraight] = DataContainer.Instance.GetHandCategorySO(HandCategory.FullStraight).scorePair;
        }
    }

    private void UpdateYachtScore(Dictionary<int, int> countMap)
    {
        if (countMap.Any(x => x.Value >= 5))
        {
            handCategoryScoreDictionary[HandCategory.Yacht] = DataContainer.Instance.GetHandCategorySO(HandCategory.Yacht).scorePair;
        }
    }

    private void UpdateSixSixScore(Dictionary<int, int> countMap)
    {
        var maxPair = countMap.OrderByDescending(x => x.Value).FirstOrDefault();
        if (maxPair.Value >= 6)
        {
            ScorePair scorePair = DataContainer.Instance.GetHandCategorySO(HandCategory.SixSix).scorePair;
            scorePair.baseScore = maxPair.Key * 111;
            handCategoryScoreDictionary[HandCategory.SixSix] = scorePair;
        }
    }
    #endregion

    #region UpdateScore
    private void UpdateCurrentRoundScore(int score)
    {
        CurrentRoundScore = score;
        PlayScore = 0;

        OnCurrentRoundScoreUpdated?.Invoke(CurrentRoundScore);
    }

    private void UpdateTargetRoundScore(int currentRound)
    {
        int roundIdx = currentRound - 1;
        if (roundIdx < 0) return;

        int baseScore = (int)(100 * Mathf.Pow(3, roundIdx / 5));
        float multiplier = 1f + roundIdx % 5 * 0.5f;
        int score = (int)(baseScore * multiplier);

        int digits = score.ToString().Length;
        if (digits > 2)
        {
            int divisor = (int)Mathf.Pow(10, digits - 2);
            TargetRoundScore = score / divisor * divisor;
        }
        else
        {
            TargetRoundScore = score;
        }

        OnTargetRoundScoreUpdated?.Invoke(TargetRoundScore);
    }
    #endregion

    #region CheckOverflow
    private bool CheckAddOverFlow(int value, int addValue)
    {
        try
        {
            checked
            {
                int result = value + addValue;
                return result < 0 || result > int.MaxValue;
            }
        }
        catch (OverflowException)
        {
            return true;
        }
    }

    private bool CheckMultiplyOverFlow(int value, int multiplyValue)
    {
        try
        {
            checked
            {
                int result = value * multiplyValue;
                return result < 0 || result > int.MaxValue;
            }
        }
        catch (OverflowException)
        {
            return true;
        }
    }
    #endregion
}
