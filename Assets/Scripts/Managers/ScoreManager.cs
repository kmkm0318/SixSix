using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    private ScorePair scorePair = new();
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
        handScoreDictionary = HandCalculator.GetHandScorePairs(playDiceValues);
        OnHandScoreUpdated?.Invoke(handScoreDictionary);
    }

    private void OnHandSelected(HandSO handSO, ScorePair scorePair)
    {
        if (GameManager.Instance.CurrentGameState != GameState.Round) return;

        ScorePair = scorePair;
        SequenceManager.Instance.ApplyParallelCoroutine();

        PlayerDiceManager.Instance.ApplyPlayDices();
        PlayerDiceManager.Instance.ApplyChaosDices();
        PlayerDiceManager.Instance.ApplyAvailityDice(handSO);

        PlayScore = SafeMultiply(ScorePair.baseScore, ScorePair.multiplier);

        ScorePair = new();
        SequenceManager.Instance.ApplyParallelCoroutine();
        UpdateCurrentRoundScore(SafeAdd(CurrentRoundScore, PlayScore));
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

        float baseScore = 500 * Mathf.Pow(6, (currentRound - 1) / 6);
        float multiplier = 1f + (currentRound - 1) % 6 * 0.5f;
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
        bool isBaseScoreZero = pair.baseScore == 0f;
        bool isMultiplierZeroOrOne = pair.multiplier == 0f || pair.multiplier == 1f;

        if (isBaseScoreZero && isMultiplierZeroOrOne) return;

        if (!isBaseScoreZero && !isMultiplierZeroOrOne)
        {
            ApplyDiceScorePairEffectAndPlayAnimation(dice, new(pair.baseScore, 1), isAvailityDice);
            ApplyDiceScorePairEffectAndPlayAnimation(dice, new(0, pair.multiplier), isAvailityDice);
            return;
        }

        var isApplied = TryApplyScorePairEffect(pair);
        if (!isApplied) return;

        SequenceManager.Instance.AddCoroutine(AnimationFunction.PlayShakeAnimation(dice.transform, false), true);
        OnScorePairApplied(pair, dice.transform, isAvailityDice);

        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public void ApplyMoneyAndPlayDiceAnimation(Dice dice, int money, bool isAvailityDice = false)
    {
        if (money == 0) return;

        SequenceManager.Instance.AddCoroutine(AnimationFunction.PlayShakeAnimation(dice.transform, false), true);
        OnMoneyAchieved(money, dice.transform, isAvailityDice);

        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private bool TryApplyScorePairEffect(ScorePair pair)
    {
        bool isBaseScoreZero = pair.baseScore == 0f;
        bool isMultiplierZeroOrOne = pair.multiplier == 0f || pair.multiplier == 1f;

        if (isBaseScoreZero && isMultiplierZeroOrOne) return false;

        if (!isBaseScoreZero)
        {
            ApplyBaseScore(pair.baseScore);
        }

        if (!isMultiplierZeroOrOne)
        {
            ApplyMultiplier(pair.multiplier);
        }

        return true;
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
