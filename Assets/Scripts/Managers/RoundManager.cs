using System;
using UnityEngine;

public class RoundManager : Singleton<RoundManager>
{
    [SerializeField] private int clearRound = 25;
    public int ClearRound => clearRound;

    public event Action<int> OnRoundStarted;
    public event Action<int> OnRoundEnded;
    public event Action<int> OnTargetScoreUpdated;

    private int currentRound = 0;
    public int CurrentRound => currentRound;

    private int targetScore;
    public int TargetScore => targetScore;

    void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Round)
        {
            StartNextRound();
        }
    }

    private void StartNextRound()
    {
        currentRound++;
        UpdateTargetScore();
        OnRoundStarted?.Invoke(CurrentRound);
    }

    private void EndCurrentRound()
    {
        OnRoundEnded?.Invoke(CurrentRound);
    }

    private void UpdateTargetScore()
    {
        int baseScore = (int)(100 * Mathf.Pow(3, currentRound / 5));
        float multiplier = 1f + currentRound % 5 * 0.5f;
        int score = (int)(baseScore * multiplier);

        int digits = (int)Mathf.Floor(Mathf.Log10(score)) + 1;
        if (digits > 2)
        {
            int divisor = (int)Mathf.Pow(10, digits - 2);
            targetScore = score / divisor * divisor;
        }
        else
        {
            targetScore = score;
        }

        OnTargetScoreUpdated?.Invoke(TargetScore);
    }
}
