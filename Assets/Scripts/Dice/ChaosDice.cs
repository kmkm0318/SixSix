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
        ScoreManager.Instance.ApplyScorePairAndPlayDiceAnimation(this, scorePair, false);

        scorePair = new(0, (FaceValueMax - FaceValue + 1) * (1f / FaceValueMax));
        ScoreManager.Instance.ApplyScorePairAndPlayDiceAnimation(this, scorePair, false);
    }

    protected override void OnShopStarted()
    {

    }

    protected override void OnShopEnded()
    {

    }

    public override void ShowToolTip()
    {
        string name = $"Chaos Dice({FaceValue})";
        string description = $"Get Score(-{FaceValue * 25}, {(FaceValueMax - FaceValue + 1) * (1f / FaceValueMax)})";

        ToolTipUI.Instance.ShowToolTip(this, transform, Vector3.down, name, description);
    }
}
