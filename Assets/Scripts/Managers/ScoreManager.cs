using System;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private double initialTargetRoundScore;
    public event Action OnCurrentRoundScoreUpdated;
    public event Action<double> OnTargetRoundScoreChanged;
    public event Action<double> OnCurrentRoundScoreChanged;
    public event Action<double> OnPlayScoreChanged;
    public event Action<ScorePair> OnScorePairChanged;
    public event Action<double> OnBaseScoreChanged;
    public event Action<double> OnMultiplierChanged;

    private double targetRoundScore = 0;
    public double TargetRoundScore
    {
        get => targetRoundScore;
        private set
        {
            if (targetRoundScore == value) return;
            targetRoundScore = value;
            OnTargetRoundScoreChanged?.Invoke(targetRoundScore);
        }
    }
    private double currentRoundScore = 0;
    public double CurrentRoundScore
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
    private double previousRoundScore = 0;
    public double PreviousRoundScore => previousRoundScore;
    private double highestRoundScore = 0;
    public double HighestRoundScore => highestRoundScore;
    private double playScore = 0;
    public double PlayScore
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
        GameManager.Instance.RegisterEvent(GameState.Round, null, OnRoundCleared);
        HandManager.Instance.OnHandScoreApplied += OnHandScoreApplied;
    }

    private void OnRoundCleared()
    {
        previousRoundScore = CurrentRoundScore;
        CurrentRoundScore = 0;
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private void OnHandScoreApplied(ScorePair scorePair)
    {
        if (GameManager.Instance.CurrentGameState != GameState.Round) return;

        ScorePair = scorePair;
        SequenceManager.Instance.ApplyParallelCoroutine();

        TriggerManager.Instance.TriggerDices();

        PlayScore = UtilityFunctions.SafeMultiply(ScorePair.baseScore, ScorePair.multiplier);

        ScorePair = new();
        SequenceManager.Instance.ApplyParallelCoroutine();
        UpdateCurrentRoundScore(UtilityFunctions.SafeAdd(CurrentRoundScore, PlayScore));
    }
    #endregion

    #region UpdateScore
    private void UpdateCurrentRoundScore(double score)
    {
        CurrentRoundScore = score;
        PlayScore = 0;
        SequenceManager.Instance.ApplyParallelCoroutine();

        OnCurrentRoundScoreUpdated?.Invoke();
    }

    public void UpdateTargetRoundScore()
    {
        int currentRound = RoundManager.Instance.CurrentRound;

        if (currentRound < 1) return;

        double baseScore = initialTargetRoundScore * Mathf.Pow(6, (currentRound - 1) / 6);
        double multiplier = 1f + (currentRound - 1) % 6 * 0.5f;
        double score = baseScore * multiplier;

        int digits = score.ToString("F0").Length;
        if (digits > 3)
        {
            double divisor = Mathf.Pow(10, digits - 3);
            TargetRoundScore = Math.Floor(score / divisor) * divisor;
        }
        else
        {
            TargetRoundScore = score;
        }
        SequenceManager.Instance.ApplyParallelCoroutine();
    }
    #endregion

    #region ApplyScorePair
    public void ApplyScorePair(ScorePair pair)
    {
        bool isBaseScoreZero = pair.baseScore == 0f;
        bool isMultiplierZeroOrOne = pair.multiplier == 0f || pair.multiplier == 1f;

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

    private void ApplyBaseScore(double value)
    {
        ScorePair tmp = ScorePair;

        tmp.baseScore = UtilityFunctions.SafeAdd(tmp.baseScore, value);

        ScorePair = tmp;
    }

    private void ApplyMultiplier(double value)
    {
        ScorePair tmp = ScorePair;

        tmp.multiplier = UtilityFunctions.SafeMultiply(tmp.multiplier, value);

        ScorePair = tmp;
    }
    #endregion
}
