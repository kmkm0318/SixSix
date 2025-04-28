using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public event Action<Dictionary<Hand, ScorePair>> OnHandScoreUpdated;
    private Dictionary<Hand, ScorePair> handScoreDictionary = new();

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
    public event Action<HandSO> OnHandApplied;


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
        HandScoreUI.Instance.OnHandSelected += OnHandSelected;
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
        UpdateHandScoreDictionary(playDiceValues);
        OnHandScoreUpdated?.Invoke(handScoreDictionary);
    }

    private void OnHandSelected(HandSO handSO, ScorePair scorePair)
    {
        ScorePair = scorePair;
        SequenceManager.Instance.ApplyParallelCoroutine();

        PlayerDiceManager.Instance.ApplyPlayDices();
        PlayerDiceManager.Instance.ApplyChaosDices();
        PlayerDiceManager.Instance.ApplyAvailityDiceOnHandApplied(handSO);
        OnHandApplied?.Invoke(handSO);

        PlayScore = SafeMultiply(ScorePair.baseScore, ScorePair.multiplier);

        ScorePair = new();
        SequenceManager.Instance.ApplyParallelCoroutine();
        UpdateCurrentRoundScore(SafeAdd(CurrentRoundScore, PlayScore));
    }
    #endregion

    #region CalculateHandScore
    private void UpdateHandScoreDictionary(List<int> diceValues)
    {
        if (diceValues == null || diceValues.Count == 0) return;

        ResetHandScore();
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

    private void ResetHandScore()
    {
        handScoreDictionary.Clear();
        foreach (var hand in DataContainer.Instance.TotalHandListSO.handList)
        {
            handScoreDictionary[hand.hand] = new ScorePair(0, 0);
        }
    }

    private void UpdateChoiceScore()
    {
        handScoreDictionary[Hand.Choice] = DataContainer.Instance.GetHandSO(Hand.Choice).scorePair;
    }

    private void UpdateFourOfAKindScore(Dictionary<int, int> countMap)
    {
        if (countMap.Any(x => x.Value >= 4))
        {
            handScoreDictionary[Hand.FourOfAKind] = DataContainer.Instance.GetHandSO(Hand.FourOfAKind).scorePair;
        }
    }

    private void UpdateFullHouseScore(Dictionary<int, int> countMap)
    {
        var hasThreeOrMore = countMap.Any(x => x.Value >= 3);
        var hasAnotherTwoOrMore = countMap.Count(x => x.Value >= 2) >= 2;
        if (hasThreeOrMore && hasAnotherTwoOrMore)
        {
            handScoreDictionary[Hand.FullHouse] = DataContainer.Instance.GetHandSO(Hand.FullHouse).scorePair;
        }
    }

    private void UpdateDoubleThreeOfAKindScore(Dictionary<int, int> countMap)
    {
        var threeOrMoreCount = countMap.Count(x => x.Value >= 3);
        if (threeOrMoreCount >= 2)
        {
            handScoreDictionary[Hand.DoubleThreeOfAKind] = DataContainer.Instance.GetHandSO(Hand.DoubleThreeOfAKind).scorePair;
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

    private void UpdateYachtScore(Dictionary<int, int> countMap)
    {
        if (countMap.Any(x => x.Value >= 5))
        {
            handScoreDictionary[Hand.Yacht] = DataContainer.Instance.GetHandSO(Hand.Yacht).scorePair;
        }
    }

    private void UpdateSixSixScore(Dictionary<int, int> countMap)
    {
        var maxPair = countMap.OrderByDescending(x => x.Value).FirstOrDefault();
        if (maxPair.Value >= 6)
        {
            ScorePair scorePair = DataContainer.Instance.GetHandSO(Hand.SixSix).scorePair;
            scorePair.baseScore *= maxPair.Key;
            scorePair.multiplier *= maxPair.Key;
            handScoreDictionary[Hand.SixSix] = scorePair;
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
        if (currentRound < 1) return;

        float baseScore = 100 * Mathf.Pow(6, (currentRound - 1) / 5);
        float multiplier = 1f + (currentRound - 1) % 5 * 0.5f;
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

    #region ApplyEffect
    public void ApplyDiceScorePairEffectAndPlayAnimation(Dice dice, ScorePair pair, bool isAvailityDice = false)
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
        bool isMultiplierZeroOrOne = pair.multiplier == 0 || pair.multiplier == 1f;

        if (isBaseScoreZero && isMultiplierZeroOrOne) return;

        if (!isBaseScoreZero)
        {
            ApplyBaseScore(pair.baseScore);
        }

        if (!isMultiplierZeroOrOne)
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
        if (float.IsInfinity(res) || float.IsNaN(res))
        {
            return float.PositiveInfinity;
        }
        else if (res < 0)
        {
            return 0;
        }
        else
        {
            return res;
        }
    }

    private float SafeMultiply(float value1, float value2)
    {
        float res = value1 * value2;
        if (float.IsInfinity(res) || float.IsNaN(res))
        {
            return float.PositiveInfinity;
        }
        else if (res < 0)
        {
            return 0;
        }
        else
        {
            return res;
        }
    }
    #endregion
}
