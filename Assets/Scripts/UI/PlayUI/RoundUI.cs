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
        RoundManager.Instance.OnCurrentRoundUpdated += OnCurrentRoundUpdated;
        ScoreManager.Instance.OnTargetRoundScoreChanged += OnTargetRoundScoreChanged;
        ScoreManager.Instance.OnCurrentRoundScoreChanged += OnCurrentRoundScoreChanged;
        ScoreManager.Instance.OnPlayScoreChanged += OnPlayScoreChanged;
        ScoreManager.Instance.OnScorePairChanged += OnScorePairChanged;
        ScoreManager.Instance.OnBaseScoreChanged += OnBaseScoreChanged;
        ScoreManager.Instance.OnMultiplierChanged += OnMultiplierChanged;
    }

    private void OnCurrentRoundUpdated(int currnetRound)
    {
        string newText = currnetRound.ToString() + "/" + RoundManager.Instance.ClearRound.ToString();

        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(currentRoundText, newText, AnimationType.Shake), true);
    }

    private void OnTargetRoundScoreChanged(int score)
    {
        string newText = score.ToString();

        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(targetRoundScoreText, newText, AnimationType.Shake), true);
    }

    private void OnCurrentRoundScoreChanged(int score)
    {
        string newText = score.ToString();

        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(currentRoundScoreText, newText, AnimationType.Shake), true);
    }

    private void OnPlayScoreChanged(int score)
    {
        string newText = score.ToString();

        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(playScoreText, newText, AnimationType.Shake), true);
    }

    private void OnScorePairChanged(ScorePair pair)
    {
        OnBaseScoreChanged(pair.baseScore);
        OnMultiplierChanged(pair.multiplier);

        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private void OnBaseScoreChanged(int score)
    {
        string newText = score.ToString();

        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(baseScoreText, newText, AnimationType.Shake), true);
    }

    private void OnMultiplierChanged(int score)
    {
        string newText = score.ToString();

        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(multiplierText, newText, AnimationType.Shake), true);
    }

    private IEnumerator UpdateTextAndPlayAnimation(TMP_Text textComponent, string newText, AnimationType animationType = AnimationType.None)
    {
        textComponent.text = newText;
        yield return AnimationManager.Instance.PlayAnimation(textComponent, animationType);
    }
}
