using System;
using UnityEngine;

public class AvailityDice : Dice
{
    private AvailityDiceSO availityDiceSO;
    public AvailityDiceSO AvailityDiceSO => availityDiceSO;
    public int SellPrice => availityDiceSO.SellPrice;

    public void Init(AvailityDiceSO availityDiceSO, Playboard playboard)
    {
        base.Init(availityDiceSO.MaxDiceValue, availityDiceSO.diceSpriteListSO, availityDiceSO.diceMaterialSO, playboard);

        this.availityDiceSO = availityDiceSO;
    }

    #region Events
    override protected void OnRollCompleted()
    {
        base.OnRollCompleted();

        if (IsInteractable && DiceInteractType == DiceInteractType.Keep && PlayerDiceManager.Instance.IsAvailityDiceAutoKeep && PlayerDiceManager.Instance.IsKeepable && FaceIndex == availityDiceSO.MaxDiceValue - 1)
        {
            IsKeeped = true;
        }
    }

    protected override void OnShopStarted()
    {
        IsInteractable = true;
        DiceInteractType = DiceInteractType.Sell;
    }

    protected override void OnShopEnded()
    {
        IsInteractable = false;
        DiceInteractType = DiceInteractType.Keep;
    }

    protected override void OnDiceEnhanceStarted()
    {
        IsInteractable = false;
    }

    protected override void OnDiceEnhanceCompleted()
    {
        IsInteractable = true;
        DiceInteractType = DiceInteractType.Sell;
    }

    protected override void OnHandEnhanceStarted()
    {
        IsInteractable = false;
    }

    protected override void OnHandEnhanceCompleted()
    {
        IsInteractable = true;
    }
    #endregion

    public override void ShowToolTip()
    {
        string name = availityDiceSO.diceName;
        string description = availityDiceSO.GetDescriptionText();
        ToolTipUI.Instance.ShowToolTip(this, transform, Vector3.down, name, description);
    }

    protected override void InitDiceInteractType()
    {
        base.InitDiceInteractType();

        if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            DiceInteractType = DiceInteractType.Sell;
        }
    }

    public bool IsTriggered(AvailityTriggerType triggerType, AvailityDiceContext context)
    {
        return IsEnabled && availityDiceSO.availityTrigger.IsTriggered(triggerType, new(this, context.playDice, context.handSO));
    }

    public void TriggerEffect()
    {
        if (availityDiceSO == null || !IsEnabled) return;

        availityDiceSO.availityEffect.TriggerEffect(new(this));
    }
}
