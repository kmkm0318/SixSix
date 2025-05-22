using UnityEngine;

public class ScorePairPanel : BasePanel
{
    [SerializeField] private AnimatedText playScoreText;
    [SerializeField] private TextPanel baseScorePanel;
    [SerializeField] private TextPanel multiplierPanel;

    public void SetPlayScore(double totalScore)
    {
        playScoreText.SetText(totalScore.ToString("0"));
    }

    public void SetBaseScore(double baseScore)
    {
        baseScorePanel.SetText(baseScore.ToString("0"));
    }

    public void SetMultiplier(double multiplier)
    {
        multiplierPanel.SetText(multiplier.ToString("0"));
    }
}