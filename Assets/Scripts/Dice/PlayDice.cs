using UnityEngine;

public class PlayDice : Dice
{
    private void Start()
    {
        ChangeFace(0);
    }

    public void ApplyScorePairs()
    {
        if (!IsEnabled) return;

        ScorePair scorePair = new(FaceValue, 0);

        ScoreManager.Instance.ApplyDiceScorePairEffectAndPlayAnimation(this, scorePair, false);

        Faces[FaceIndex].ApplyDiceFaceValue(this, false);
    }

    protected override void OnShopStarted()
    {
        DiceEnhanceManager.Instance.OnEnhanceStarted += OnEnhanceStarted;
        DiceEnhanceManager.Instance.OnEnhanceCompleted += OnEnhanceCompleted;
    }

    protected override void OnShopEnded()
    {
        DiceEnhanceManager.Instance.OnEnhanceStarted -= OnEnhanceStarted;
        DiceEnhanceManager.Instance.OnEnhanceCompleted -= OnEnhanceCompleted;
    }

    private void OnEnhanceStarted()
    {
        IsInteractable = true;
    }

    private void OnEnhanceCompleted()
    {
        IsInteractable = false;
    }

    public override DiceHighlightType GetHighlightType()
    {
        var type = base.GetHighlightType();

        if (type != DiceHighlightType.None) return type;

        if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            return DiceHighlightType.Enhance;
        }
        else
        {
            return DiceHighlightType.None;
        }
    }

    public override void ShowToolTip()
    {
        string name = $"PlayDice({FaceValue})";
        string description = $"Get Score(+{FaceValue})" + Faces[FaceIndex].GetDescriptionText();

        ToolTipUI.Instance.ShowToolTip(this, transform, Vector3.down, name, description);
    }
}
