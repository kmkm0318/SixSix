using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
        RoundManager.Instance.CurrentRoundUpdated += CurrentRoundUpdated;
        ScoreManager.Instance.TargetRoundScoreUpdated += TargetRoundScoreUpdated;
        ScoreManager.Instance.CurrentRoundScoreUpdated += CurrentRoundScoreUpdated;
        ScoreManager.Instance.PlayScoreUpdated += PlayScoreUpdated;
        ScoreManager.Instance.ScorePairUpdated += ScorePairUpdated;
        ScoreManager.Instance.BaseScoreUpdated += BaseScoreUpdated;
        ScoreManager.Instance.MultiplierUpdated += MultiplierUpdated;
    }

    private void CurrentRoundUpdated(int currnetRound)
    {
        string newText = currnetRound.ToString() + "/" + RoundManager.Instance.ClearRound.ToString();

        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(currentRoundText, newText, UIAnimationType.Shake));
    }

    private void TargetRoundScoreUpdated(int score)
    {
        string newText = score.ToString();

        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(targetRoundScoreText, newText, UIAnimationType.Shake));
    }

    private void CurrentRoundScoreUpdated(int score)
    {
        string newText = score.ToString();

        SequenceManager.Instance.AddParallelCoroutine(UpdateTextAndPlayAnimation(currentRoundScoreText, newText, UIAnimationType.Shake));
        if (score == 0)
        {
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }

    private void PlayScoreUpdated(int score)
    {
        string newText = score.ToString();

        SequenceManager.Instance.AddParallelCoroutine(UpdateTextAndPlayAnimation(playScoreText, newText, UIAnimationType.Shake));
        if (score == 0)
        {
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }

    private void ScorePairUpdated(ScorePair pair)
    {
        string baseScore = pair.baseScore.ToString();
        string multiplier = pair.multiplier.ToString();

        SequenceManager.Instance.AddParallelCoroutine(UpdateTextAndPlayAnimation(multiplierText, multiplier, UIAnimationType.Shake));
        SequenceManager.Instance.AddParallelCoroutine(UpdateTextAndPlayAnimation(baseScoreText, baseScore, UIAnimationType.Shake));
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private void BaseScoreUpdated(int score)
    {
        throw new NotImplementedException();
    }

    private void MultiplierUpdated(int score)
    {
        throw new NotImplementedException();
    }

    private IEnumerator UpdateTextAndPlayAnimation(TMP_Text textComponent, string newText, UIAnimationType animationType = UIAnimationType.None)
    {
        textComponent.text = newText;
        yield return UIAnimationManager.Instance.PlayAnimation(textComponent, animationType);
    }
}
