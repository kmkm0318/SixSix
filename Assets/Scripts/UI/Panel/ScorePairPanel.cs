using UnityEngine;

public class ScorePairPanel : BasePanel
{
    [SerializeField] private AnimatedText _playScoreText;
    [SerializeField] private TextPanel _baseScorePanel;
    [SerializeField] private TextPanel _multiplierPanel;

    public void SetPlayScore(double totalScore)
    {
        _playScoreText.SetText(totalScore.ToString("0"));
    }

    public void SetBaseScore(double baseScore)
    {
        _baseScorePanel.SetText(baseScore.ToString("0"));
    }

    public void SetMultiplier(double multiplier)
    {
        _multiplierPanel.SetText(multiplier.ToString("0"));
    }
}