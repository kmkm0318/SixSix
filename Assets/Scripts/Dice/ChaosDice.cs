using UnityEngine;

public class ChaosDice : Dice
{
    ScorePair scorePair = new();

    private void Start()
    {
        ChangeFace(0);
    }

    private void UpdateScorePair()
    {
        scorePair.baseScore = -FaceValue * 25;
        scorePair.multiplier = (FaceValueMax - FaceValue + 1) * (1f / FaceValueMax);
    }

    public void ApplyScorePairs()
    {
        UpdateScorePair();
        ScoreManager.Instance.ApplyDiceScorePairEffectAndPlayAnimation(this, scorePair, false);
    }

    public override void ShowToolTip()
    {
        UpdateScorePair();

        string name = $"Chaos Dice({FaceValue})";
        string description = $"Get {scorePair}";

        ToolTipUI.Instance.ShowToolTip(this, transform, Vector3.down, name, description);
    }
}
