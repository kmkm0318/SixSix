using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public event Action<Dictionary<HandCategory, ScorePair>> OnHandCategoryScoreUpdated;
    private Dictionary<HandCategory, ScorePair> handCategoryScoreDictionary = new();

    public event Action<float> OnTargetRoundScoreUpdated;
    public event Action<float> OnCurrentRoundScoreUpdated;
    public event Action<float> OnTargetRoundScoreChanged;
    public event Action<float> OnCurrentRoundScoreChanged;
    public event Action<float> OnPlayScoreChanged;
    public event Action<ScorePair> OnScorePairChanged;
    public event Action<float> OnBaseScoreChanged;
    public event Action<float> OnMultiplierChanged;
    public event Action<ScorePair, Transform, bool> OnScorePairApplied;
    public event Action<int, Transform, bool> OnMoneyAchieved;


    private float targetRoundScore = 0;
    public float TargetRoundScore
    {
        get => targetRoundScore;
        private set
        {
            if (targetRoundScore == value) return;
            targetRoundScore = value;
            OnTargetRoundScoreChanged?.Invoke(targetRoundScore);
        }
    }

    private float currentRoundScore = 0;
    public float CurrentRoundScore
    {
        get => currentRoundScore;
        private set
        {
            if (currentRoundScore == value) return;
            currentRoundScore = value;
            if (highestRoundScore < currentRoundScore)
            {
                highestRoundScore = currentRoundScore;
            }
            OnCurrentRoundScoreChanged?.Invoke(currentRoundScore);
        }
    }

    private float highestRoundScore = 0;
    public float HighestRoundScore => highestRoundScore;

    private float playScore = 0;
    public float PlayScore
    {
        get => playScore;
        private set
        {
            if (playScore == value) return;
            playScore = value;
            OnPlayScoreChanged?.Invoke(playScore);
        }
    }

    private ScorePair scorePair = new(0, 0);
    public ScorePair ScorePair
    {
        get => scorePair;
        private set
        {
            if (scorePair.baseScore == value.baseScore && scorePair.multiplier == value.multiplier) return;

            bool baseScoreChanged = scorePair.baseScore != value.baseScore;
            bool multiplierChanged = scorePair.multiplier != value.multiplier;

            scorePair = value;

            if (baseScoreChanged && multiplierChanged)
            {
                OnScorePairChanged?.Invoke(scorePair);
            }
            else if (baseScoreChanged)
            {
                OnBaseScoreChanged?.Invoke(scorePair.baseScore);
            }
            else if (multiplierChanged)
            {
                OnMultiplierChanged?.Invoke(scorePair.multiplier);
            }
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
        CurrentRoundScore = 0;
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private void OnRollCompleted()
    {
        var playDiceValues = PlayerDiceManager.Instance.GetOrderedPlayDiceValues();
        UpdateHandCategoryScoreDictionary(playDiceValues);
        OnHandCategoryScoreUpdated?.Invoke(handCategoryScoreDictionary);
    }

    private void OnHandCategorySelected(HandCategorySO handCategorySO, ScorePair scorePair)
    {
        ScorePair = scorePair;
        SequenceManager.Instance.ApplyParallelCoroutine();

        PlayerDiceManager.Instance.ApplyPlayDices();
        PlayerDiceManager.Instance.ApplyAvailityDiceOnHandCategoryApplied(handCategorySO);

        PlayScore = SafeMultiply(ScorePair.baseScore, ScorePair.multiplier);

        ScorePair = new();
        SequenceManager.Instance.ApplyParallelCoroutine();
        UpdateCurrentRoundScore(SafeAdd(CurrentRoundScore, PlayScore));
    }
    #endregion

    #region CalculateHandCategoryScore
    private void UpdateHandCategoryScoreDictionary(List<int> diceValues)
    {
        if (diceValues == null || diceValues.Count == 0) return;

        ResetHandCategoryScore();
        var countMap = GetCountMap(diceValues);

        UpdateChoiceScore();
        UpdateFourOfAKindScore(countMap);
        UpdateFullHouseScore(countMap);
        UpdateDoubleThreeOfAKindScore(countMap);
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

    private void UpdateChoiceScore()
    {
        handCategoryScoreDictionary[HandCategory.Choice] = DataContainer.Instance.GetHandCategorySO(HandCategory.Choice).scorePair;
    }

    private void UpdateFourOfAKindScore(Dictionary<int, int> countMap)
    {
        if (countMap.Any(x => x.Value >= 4))
        {
            handCategoryScoreDictionary[HandCategory.FourOfAKind] = DataContainer.Instance.GetHandCategorySO(HandCategory.FourOfAKind).scorePair;
        }
    }

    private void UpdateFullHouseScore(Dictionary<int, int> countMap)
    {
        var hasThreeOrMore = countMap.Any(x => x.Value >= 3);
        var hasAnotherTwoOrMore = countMap.Count(x => x.Value >= 2) >= 2;
        if (hasThreeOrMore && hasAnotherTwoOrMore)
        {
            handCategoryScoreDictionary[HandCategory.FullHouse] = DataContainer.Instance.GetHandCategorySO(HandCategory.FullHouse).scorePair;
        }
    }

    private void UpdateDoubleThreeOfAKindScore(Dictionary<int, int> countMap)
    {
        var threeOrMoreCount = countMap.Count(x => x.Value >= 3);
        if (threeOrMoreCount >= 2)
        {
            handCategoryScoreDictionary[HandCategory.DoubleThreeOfAKind] = DataContainer.Instance.GetHandCategorySO(HandCategory.DoubleThreeOfAKind).scorePair;
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
            scorePair.baseScore *= maxPair.Key;
            scorePair.multiplier *= maxPair.Key;
            handCategoryScoreDictionary[HandCategory.SixSix] = scorePair;
        }
    }
    #endregion

    #region UpdateScore
    private void UpdateCurrentRoundScore(float score)
    {
        CurrentRoundScore = score;
        PlayScore = 0;
        SequenceManager.Instance.ApplyParallelCoroutine();

        OnCurrentRoundScoreUpdated?.Invoke(CurrentRoundScore);
    }

    private void UpdateTargetRoundScore(int currentRound)
    {
        int roundIdx = currentRound - 1;
        if (roundIdx < 0) return;

        float baseScore = 100 * Mathf.Pow(6, roundIdx);
        float multiplier = 1f + roundIdx % 5 * 0.5f;
        float score = baseScore * multiplier;

        int digits = score.ToString("F0").Length;
        if (digits > 3)
        {
            float divisor = Mathf.Pow(10, digits - 3);
            TargetRoundScore = Mathf.Floor(score / divisor) * divisor;
        }
        else
        {
            TargetRoundScore = score;
        }
        SequenceManager.Instance.ApplyParallelCoroutine();

        OnTargetRoundScoreUpdated?.Invoke(TargetRoundScore);
    }
    #endregion

    #region ApplyDiceEffect
    public void ApplyScorePairAndPlayDiceAnimation(Dice dice, ScorePair pair, bool isAvailityDice = false)
    {
        if (pair.baseScore == 0 && pair.multiplier == 0) return;

        ApplyScorePairEffect(pair);

        SequenceManager.Instance.AddCoroutine(AnimationManager.Instance.PlayShakeAnimation(dice.transform), true);
        OnScorePairApplied(pair, dice.transform, isAvailityDice);

        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public void ApplyMoneyAndPlayDiceAnimation(Dice dice, int money, bool isAvailityDice = false)
    {
        if (money == 0) return;

        SequenceManager.Instance.AddCoroutine(AnimationManager.Instance.PlayShakeAnimation(dice.transform), true);
        OnMoneyAchieved(money, dice.transform, isAvailityDice);

        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private void ApplyScorePairEffect(ScorePair pair)
    {
        bool isBaseScoreZero = pair.baseScore == 0;
        bool isMultiplierZero = pair.multiplier == 0;

        if (isBaseScoreZero && isMultiplierZero) return;

        if (!isBaseScoreZero)
        {
            ApplyBaseScore(pair.baseScore);
        }

        if (!isMultiplierZero)
        {
            ApplyMultiplier(pair.multiplier);
        }
    }

    private void ApplyBaseScore(float value)
    {
        ScorePair tmp = scorePair;

        tmp.baseScore = SafeAdd(tmp.baseScore, value);

        ScorePair = tmp;
    }

    private void ApplyMultiplier(float value)
    {
        ScorePair tmp = scorePair;

        tmp.multiplier = SafeMultiply(tmp.multiplier, value);

        ScorePair = tmp;
    }
    #endregion

    #region Arithmatic
    private float SafeAdd(float value1, float value2)
    {
        float res = value1 + value2;
        if (float.IsInfinity(res) || res < 0)
        {
            return float.PositiveInfinity;
        }
        else
        {
            return res;
        }
    }

    private float SafeMultiply(float value1, float value2)
    {
        float res = value1 * value2;
        if (float.IsInfinity(res) || res < 0)
        {
            return float.PositiveInfinity;
        }
        else
        {
            return res;
        }
    }
    #endregion
}
