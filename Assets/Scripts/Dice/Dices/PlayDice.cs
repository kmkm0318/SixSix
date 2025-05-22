using UnityEngine;

public class PlayDice : Dice
{
    #region Events
    protected override void OnDiceEnhanceStarted()
    {
        DiceInteractType = DiceInteractType.Enhance;
        GameManager.Instance.RegisterEvent(GameState.Play, EnableInteraction);
    }

    protected override void OnDiceEnhanceCompleted()
    {
        DiceInteractType = DiceInteractType.Keep;
        GameManager.Instance.UnregisterEvent(GameState.Play, EnableInteraction);
    }

    private void EnableInteraction()
    {
        IsInteractable = true;
    }
    #endregion

    public void ApplyScorePairs()
    {
        if (!IsEnabled) return;

        ScorePair scorePair = new(DiceValue, 1);
        TriggerManager.Instance.ApplyTriggerEffect(transform, Vector3.up, scorePair);

        Faces[FaceIndex].ApplyFaceValue(this, false);
    }

    public override void ShowToolTip()
    {
        string name = $"PlayDice({DiceValue})";

        ScorePair scorePair = new(DiceValue, 1);
        string description = $"Get {scorePair}" + Faces[FaceIndex].GetDescriptionText();

        ToolTipUI.Instance.ShowToolTip(transform, Vector2.down, name, description);
    }
}
