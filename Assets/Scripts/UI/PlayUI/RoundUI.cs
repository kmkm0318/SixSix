using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentRoundText;
    [SerializeField] private TMP_Text targetRoundScoreText;
    [SerializeField] private TMP_Text currentRoundScoreText;
    [SerializeField] private TMP_Text playScoreText;
    [SerializeField] private TMP_Text baseScoreText;
    [SerializeField] private TMP_Text multiplierText;

    private void Start()
    {
        ResetUI();
        RegisterEvents();
    }

    private void ResetUI()
    {
        targetRoundScoreText.text = "0";
        currentRoundScoreText.text = "0";
        playScoreText.text = "0";
        baseScoreText.text = "0";
        multiplierText.text = "0";
    }

    private void RegisterEvents()
    {
        RoundManager.Instance.OnRoundStarted += OnRoundStarted;
        ScoreManager.Instance.OnTargetRoundScoreUpdated += OnTargetRoundScoreUpdated;
        ScoreManager.Instance.OnCurrentRoundScoreUpdated += OnCurrentRoundScoreUpdated;
    }

    private void OnRoundStarted(int score)
    {
        currentRoundText.text = score.ToString() + "/" + RoundManager.Instance.ClearRound.ToString();
        if (currentRoundText.TryGetComponent(out RectTransform rectTransform))
        {
            UIAnimationManager.Instance.AddAnimation(rectTransform, UIAnimationManager.AnimationType.Shake);
        }
    }

    private void OnTargetRoundScoreUpdated(int score)
    {
        targetRoundScoreText.text = score.ToString();
        if (targetRoundScoreText.TryGetComponent(out RectTransform rectTransform))
        {
            UIAnimationManager.Instance.AddAnimation(rectTransform, UIAnimationManager.AnimationType.Shake);
        }
    }

    private void OnCurrentRoundScoreUpdated(int score)
    {
        currentRoundScoreText.text = score.ToString();
        if (currentRoundScoreText.TryGetComponent(out RectTransform rectTransform))
        {
            UIAnimationManager.Instance.AddAnimation(rectTransform, UIAnimationManager.AnimationType.Shake);
        }
    }
}
