using UnityEngine;

public class GambleDice : Dice
{
    private GambleDiceSO gambleDiceSO;

    public void Init(GambleDiceSO gambleDiceSO, Playboard playboard)
    {
        base.Init(gambleDiceSO.MaxDiceValue, gambleDiceSO.diceSpriteListSO, gambleDiceSO.shaderDataSO, playboard);

        this.gambleDiceSO = gambleDiceSO;
    }

    #region Events
    protected override void OnPlayStarted()
    {
        base.OnPlayStarted();
        IsInteractable = false;
    }

    protected override void OnRoundStarted()
    {
        base.OnRoundStarted();
        IsInteractable = false;
    }

    override protected void OnRollCompleted()
    {
        base.OnRollCompleted();
        IsInteractable = false;
    }

    protected override void OnShopStarted()
    {
        IsInteractable = false;
    }

    protected override void OnShopEnded()
    {
        IsInteractable = false;
    }

    protected override void OnDiceEnhanceStarted()
    {
        IsInteractable = false;
    }

    protected override void OnDiceEnhanceCompleted()
    {
        IsInteractable = false;
    }

    protected override void OnHandEnhanceStarted()
    {
        IsInteractable = false;
    }

    protected override void OnHandEnhanceCompleted()
    {
        IsInteractable = false;
    }
    #endregion

    protected override void InitDiceInteractType()
    {
        IsInteractable = false;
    }

    public void TriggerEffect()
    {
        if (gambleDiceSO.IsTriggered(this))
        {
            gambleDiceSO.TriggerEffect(this);
        }
    }

    public override void ShowToolTip()
    {
        string name = gambleDiceSO.DiceName;
        string description = gambleDiceSO.GetDescriptionText();
        ToolTipUI.Instance.ShowToolTip(transform, Vector2.down, name, description, ToolTipTag.Gamble_Dice);
    }
}
