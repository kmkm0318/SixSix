using UnityEngine;

public class ChaosDice : Dice
{
    private void Start()
    {
        ChangeFace(0);
    }

    public void ApplyScorePairs()
    {
        ScorePair scorePair;

        scorePair = new(-FaceValue * 25, 0);
        ScoreManager.Instance.ApplyDiceScorePairEffectAndPlayAnimation(this, scorePair, false);

        scorePair = new(0, (FaceValueMax - FaceValue + 1) * (1f / FaceValueMax));
        ScoreManager.Instance.ApplyDiceScorePairEffectAndPlayAnimation(this, scorePair, false);
    }

    public override void ShowToolTip()
    {
        string name = $"Chaos Dice({FaceValue})";
        string description = $"Get Score(-{FaceValue * 25}, x{(FaceValueMax - FaceValue + 1) * (1f / FaceValueMax)})";

        ToolTipUI.Instance.ShowToolTip(this, transform, Vector3.down, name, description);
    }
}
