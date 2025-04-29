using UnityEngine;

public class AvailityDice : Dice
{
    private AvailityDiceSO availityDiceSO;
    public AvailityDiceSO AvailityDiceSO => availityDiceSO;
    public int SellPrice => availityDiceSO.sellPrice;

    public void Init(AvailityDiceSO availityDiceSO, Playboard playboard)
    {
        base.Init(availityDiceSO.maxFaceValue, availityDiceSO.diceFaceSpriteListSO, playboard);

        this.availityDiceSO = availityDiceSO;
    }

    #region Events
    override protected void OnRollCompleted()
    {
        base.OnRollCompleted();

        if (PlayerDiceManager.Instance.IsAvailityDiceAutoKeep)
        {
            if (FaceIndex == availityDiceSO.maxFaceValue - 1)
            {
                IsKeeped = true;
            }
        }
    }

    protected override void OnShopStarted()
    {
        IsInteractable = true;
    }

    protected override void OnShopEnded()
    {
        IsInteractable = false;
    }
    #endregion

    public bool IsTriggered(AvailityDiceContext context)
    {
        return IsEnabled && availityDiceSO.availityTrigger.IsTriggered(new(this, context.playDice, context.handSO));
    }

    public void ApplyEffect()
    {
        if (availityDiceSO == null || !IsEnabled) return;

        availityDiceSO.availityEffect.ApplyEffect(new(this));
    }

    public override DiceHighlightType GetHighlightType()
    {
        var type = base.GetHighlightType();

        if (type != DiceHighlightType.None) return type;

        if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            return DiceHighlightType.Sell;
        }
        else
        {
            return DiceHighlightType.None;
        }
    }

    public override void ShowToolTip()
    {
        string name = availityDiceSO.diceName;
        string description = availityDiceSO.GetDescriptionText();
        ToolTipUI.Instance.ShowToolTip(this, transform, Vector3.down, name, description);
    }
}
