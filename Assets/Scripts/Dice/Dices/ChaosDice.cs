using UnityEngine;

public class ChaosDice : Dice
{
    private ScorePair scorePair = new();

    private void UpdateScorePair()
    {
        scorePair.baseScore = -DiceValue * 25;
        scorePair.multiplier = (DiceValueMax - DiceValue + 1) * (1f / DiceValueMax);
    }

    public void ApplyScorePairs()
    {
        UpdateScorePair();
        TriggerManager.Instance.ApplyTriggerEffect(transform, Vector3.up, scorePair);
    }

    public override void ShowToolTip()
    {
        UpdateScorePair();

        string name = DiceManager.Instance.ChaosDiceName.GetLocalizedString();
        var descriptionString = DiceManager.Instance.GetScoreDescription;
        descriptionString.Arguments = new object[] { scorePair };
        descriptionString.RefreshString();
        string description = descriptionString.GetLocalizedString();

        ToolTipUIEvents.TriggerOnToolTipShowRequested(transform, Vector2.down, name, description, ToolTipTag.ChaosDice);
    }
}
