using UnityEngine;

public class PlayDice : Dice
{
    private void Start()
    {
        ChangeFace(0);
    }

    #region Events
    protected override void OnDiceEnhanceStarted()
    {
        IsInteractable = true;
        DiceInteractType = DiceInteractType.Enhance;
    }

    protected override void OnDiceEnhanceCompleted()
    {
        IsInteractable = false;
    }

    protected override void OnHandEnhanceStarted()
    {
        IsInteractable = false;
        DiceInteractType = DiceInteractType.Keep;
    }

    protected override void OnHandEnhanceCompleted()
    {
        IsInteractable = false;
    }
    #endregion

    public void ApplyScorePairs()
    {
        if (!IsEnabled) return;

        ScoreManager.Instance.ApplyDiceScorePairEffectAndPlayAnimation(this, new(FaceValue, 1), false);

        Faces[FaceIndex].ApplyDiceFaceValue(this, false);
    }

    public override void ShowToolTip()
    {
        string name = $"PlayDice({FaceValue})";
        string description = $"Get Score(+{FaceValue})" + Faces[FaceIndex].GetDescriptionText();

        ToolTipUI.Instance.ShowToolTip(this, transform, Vector3.down, name, description);
    }
}
