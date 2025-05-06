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
        currentRoundText.text = $"0/{RoundManager.Instance.ClearRound}";
        targetRoundScoreText.text = "0";
        currentRoundScoreText.text = "0";
        playScoreText.text = "0";
        baseScoreText.text = "0";
        multiplierText.text = "0";
    }

    private void RegisterEvents()
    {
        RoundManager.Instance.OnCurrentRoundChanged += OnCurrentRoundChanged;
        ScoreManager.Instance.OnTargetRoundScoreChanged += OnTargetRoundScoreChanged;
        ScoreManager.Instance.OnCurrentRoundScoreChanged += OnCurrentRoundScoreChanged;
        ScoreManager.Instance.OnPlayScoreChanged += OnPlayScoreChanged;
        ScoreManager.Instance.OnScorePairChanged += OnScorePairChanged;
        ScoreManager.Instance.OnBaseScoreChanged += OnBaseScoreChanged;
        ScoreManager.Instance.OnMultiplierChanged += OnMultiplierChanged;
    }

    private void OnCurrentRoundChanged(int currentRound)
    {
        string newText = $"{currentRound}/{RoundManager.Instance.ClearRound}";

        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(currentRoundText, newText), true);
    }

    private void OnTargetRoundScoreChanged(float score)
    {
        UpdateScoreText(targetRoundScoreText, score);
    }

    private void OnCurrentRoundScoreChanged(float score)
    {
        UpdateScoreText(currentRoundScoreText, score);
    }

    private void OnPlayScoreChanged(float score)
    {
        UpdateScoreText(playScoreText, score);
    }

    private void OnScorePairChanged(ScorePair pair)
    {
        UpdateScoreText(baseScoreText, pair.baseScore);
        UpdateScoreText(multiplierText, pair.multiplier);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private void OnBaseScoreChanged(float score)
    {
        UpdateScoreText(baseScoreText, score);
    }

    private void OnMultiplierChanged(float score)
    {
        UpdateScoreText(multiplierText, score);
    }

    private void UpdateScoreText(TMP_Text textComponent, float score)
    {
        string newText = UtilityFunctions.FormatNumber(score);
        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(textComponent, newText), true);
    }

    private IEnumerator UpdateTextAndPlayAnimation(TMP_Text textComponent, string newText)
    {
        textComponent.text = newText;
        yield return AnimationFunction.PlayShakeAnimation(textComponent.transform);
    }
}
