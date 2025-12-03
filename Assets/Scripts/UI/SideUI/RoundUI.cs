using System.Collections;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    [SerializeField] private AnimatedText currentRoundText;
    [SerializeField] private AnimatedText targetRoundScoreText;
    [SerializeField] private AnimatedText currentRoundScoreText;
    [SerializeField] private AnimatedText playScoreText;
    [SerializeField] private AnimatedText baseScoreText;
    [SerializeField] private AnimatedText multiplierText;

    private void Start()
    {
        ResetUI();
        RegisterEvents();
    }

    private void ResetUI()
    {
        currentRoundText.SetText($"0/{RoundManager.Instance.ClearRound}");
        targetRoundScoreText.SetText("0");
        currentRoundScoreText.SetText("0");
        playScoreText.SetText("0");
        baseScoreText.SetText("0");
        multiplierText.SetText("0");
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
        SequenceManager.Instance.AddCoroutine(() => AudioManager.Instance.PlaySFX(SFXType.DiceTrigger), true);
    }

    private void OnTargetRoundScoreChanged(double score)
    {
        UpdateScoreText(targetRoundScoreText, score);
    }

    private void OnCurrentRoundScoreChanged(double score)
    {
        UpdateScoreText(currentRoundScoreText, score);
        SequenceManager.Instance.AddCoroutine(() => AudioManager.Instance.PlaySFX(SFXType.DiceTrigger), true);
    }

    private void OnPlayScoreChanged(double score)
    {
        UpdateScoreText(playScoreText, score);
        SequenceManager.Instance.AddCoroutine(() => AudioManager.Instance.PlaySFX(SFXType.DiceTrigger), true);
    }

    private void OnScorePairChanged(ScorePair pair)
    {
        UpdateScoreText(baseScoreText, pair.baseScore);
        UpdateScoreText(multiplierText, pair.multiplier);
    }

    private void OnBaseScoreChanged(double score)
    {
        UpdateScoreText(baseScoreText, score);
    }

    private void OnMultiplierChanged(double score)
    {
        UpdateScoreText(multiplierText, score);
    }

    private void UpdateScoreText(AnimatedText textComponent, double score)
    {
        string newText = UtilityFunctions.FormatNumber(score);
        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(textComponent, newText), true);
    }

    private IEnumerator UpdateTextAndPlayAnimation(AnimatedText textComponent, string newText)
    {
        textComponent.SetText(newText);
        yield return AnimationFunction.ShakeAnimation(textComponent.transform);
    }
}
