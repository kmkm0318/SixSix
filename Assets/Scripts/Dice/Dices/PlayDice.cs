using UnityEngine;

public class PlayDice : Dice
{
    #region Events
    protected override void OnDiceEnhanceStarted()
    {
        DiceInteractType = DiceInteractionType.Enhance;
        GameManager.Instance.RegisterEvent(GameState.Play, EnableInteraction);
    }

    protected override void OnDiceEnhanceCompleted()
    {
        DiceInteractType = DiceInteractionType.Keep;
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
        var name = DiceManager.Instance.PlayDiceName.GetLocalizedString();

        ScorePair scorePair = new(DiceValue, 1);
        var descriptionString = DiceManager.Instance.GetScoreDescription;
        descriptionString.Arguments = new object[] { scorePair };
        descriptionString.RefreshString();
        string description = descriptionString.GetLocalizedString();
        string faceDescription = Faces[FaceIndex].GetDescriptionText();
        if (!string.IsNullOrEmpty(faceDescription))
        {
            description += "\n" + faceDescription;
        }

        ToolTipUIEvents.TriggerOnToolTipShowRequested(transform, Vector2.down, name, description, ToolTipTag.PlayDice);
    }
}
