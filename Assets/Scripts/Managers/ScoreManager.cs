using System;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private double initialTargetRoundScore = 0;
    private double targetRoundScore = 0;
    private double currentRoundScore = 0;
    private double previousRoundScore = 0;
    private double highestRoundScore = 0;
    private double playScore = 0;
    private ScorePair scorePair = new(0, 0);

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
    public double PreviousRoundScore => previousRoundScore;
    public double HighestRoundScore => highestRoundScore;
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

    public event Action<double> OnTargetRoundScoreChanged;
    public event Action<double> OnCurrentRoundScoreChanged;
    public event Action<double> OnPlayScoreChanged;
    public event Action<ScorePair> OnScorePairChanged;
    public event Action<double> OnBaseScoreChanged;
    public event Action<double> OnMultiplierChanged;

    private void Start()
    {
        RegisterEvents();
        initialTargetRoundScore = DataContainer.Instance.CurrentPlayerStat.initialTargetRoundScore;
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

        GameManager.Instance.ExitState(GameState.Play);
    }
    #endregion

    #region UpdateScore
    private void UpdateCurrentRoundScore(double score)
    {
        CurrentRoundScore = score;
        PlayScore = 0;
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public void UpdateTargetRoundScore()
    {
        int currentRound = RoundManager.Instance.CurrentRound;

        if (currentRound < 1) return;

        //매 라운드마다 baseScore에서 50%씩 증가하는 효과. BaseScore는 6라운드마다 18배 증가. 이는 보스 라운드 클리어 시 6배 증가하는 효과.

        //6 라운드마다 18배 증가. 보스 라운드 클리어 시 6배 증가하는 효과
        double baseScore = initialTargetRoundScore * Mathf.Pow(18, (currentRound - 1) / 6);

        //6 라운드 안에서는 1라운드당 50%씩 증가. 보스 라운드까지 250% 증가하는 효과
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
