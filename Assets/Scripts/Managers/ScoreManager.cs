using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public event Action<Dictionary<HandCategory, ScorePair>> OnHandCategoryScoreUpdated;
    private Dictionary<HandCategory, ScorePair> handCategoryScoreDictionary = new();

    public event Action<int> OnTargetRoundScoreUpdated;
    public event Action<int> OnCurrentRoundScoreUpdated;
    public event Action<int> OnTargetRoundScoreChanged;
    public event Action<int> OnCurrentRoundScoreChanged;
    public event Action<int> OnPlayScoreChanged;
    public event Action<ScorePair> OnScorePairChanged;
    public event Action<int> OnBaseScoreChanged;
    public event Action<int> OnMultiplierChanged;
    public event Action<ScorePair, Transform, bool> OnScorePairApplied;
    public event Action<int, Transform, bool> OnMoneyAchieved;


    private int targetRoundScore = 0;
    public int TargetRoundScore
    {
        get => targetRoundScore;
        private set
        {
            if (targetRoundScore == value) return;
            targetRoundScore = value;
            OnTargetRoundScoreChanged?.Invoke(targetRoundScore);
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
            OnCurrentRoundScoreChanged?.Invoke(currentRoundScore);
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

        if (CheckMultiplyOverFlow(scorePair.baseScore, scorePair.multiplier))
        {
            PlayScore = int.MaxValue;
        }
        else
        {
            PlayScore = ScorePair.baseScore * ScorePair.multiplier;
        }

        ScorePair = new();
        SequenceManager.Instance.ApplyParallelCoroutine();

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
    private void UpdateCurrentRoundScore(int score)
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

        int baseScore = (int)(100 * Mathf.Pow(6, roundIdx / 5));
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

    private void ApplyBaseScore(int value)
    {
        ScorePair tmp = scorePair;

        if (CheckAddOverFlow(tmp.baseScore, value))
        {
            tmp.baseScore = int.MaxValue;
        }
        else
        {
            tmp.baseScore += value;
        }

        ScorePair = tmp;
    }

    private void ApplyMultiplier(int value)
    {
        ScorePair tmp = scorePair;

        if (CheckMultiplyOverFlow(tmp.multiplier, value))
        {
            tmp.multiplier = int.MaxValue;
        }
        else
        {
            tmp.multiplier *= value;
        }

        ScorePair = tmp;
    }
    #endregion

    #region CheckOverflow
    private bool CheckAddOverFlow(int value, int addValue)
    {
        if (value == 0 || addValue == 0) return false;

        return value > int.MaxValue - addValue || addValue > int.MaxValue - value;
    }

    private bool CheckMultiplyOverFlow(int value, int multiplyValue)
    {
        if (value == 0 || multiplyValue == 0) return false;

        return value > int.MaxValue / multiplyValue || multiplyValue > int.MaxValue / value;
    }
    #endregion
}
