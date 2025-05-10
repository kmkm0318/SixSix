using UnityEngine;

public class ChaosDice : Dice
{
    private ScorePair scorePair = new();

    private void Start()
    {
        ChangeFace(0);
    }

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

        string name = $"Chaos Dice({DiceValue})";
        string description = $"Get {scorePair}";

        ToolTipUI.Instance.ShowToolTip(this, transform, Vector3.down, name, description);
    }
}
