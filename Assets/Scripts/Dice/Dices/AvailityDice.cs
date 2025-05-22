using UnityEngine;

public class AvailityDice : Dice
{
    private AvailityDiceSO availityDiceSO;
    public AvailityDiceSO AvailityDiceSO => availityDiceSO;
    public int SellPrice => availityDiceSO.SellPrice;

    public void Init(AvailityDiceSO availityDiceSO, Playboard playboard)
    {
        base.Init(availityDiceSO.MaxDiceValue, availityDiceSO.diceSpriteListSO, availityDiceSO.shaderDataSO, playboard);

        this.availityDiceSO = Instantiate(availityDiceSO);
        this.availityDiceSO.Init();
    }

    #region Events
    override protected void OnRollCompleted()
    {
        base.OnRollCompleted();

        if (IsInteractable && DiceInteractType == DiceInteractType.Keep && DiceManager.Instance.IsAvailityDiceAutoKeep && DiceManager.Instance.IsKeepable)
        {
            if (availityDiceSO && availityDiceSO.autoKeepType == AbailityDiceAutoKeepType.High)
            {
                if (DiceValue >= availityDiceSO.MaxDiceValue)
                {
                    IsKeeped = true;
                }
            }
            else
            {
                if (DiceValue <= 1)
                {
                    IsKeeped = true;
                }
            }
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
        GameManager.Instance.RegisterEvent(GameState.Roll, null, DisableDiceInteraction);
    }

    protected override void OnDiceEnhanceCompleted()
    {
        GameManager.Instance.UnregisterEvent(GameState.Roll, null, DisableDiceInteraction);
    }

    protected override void OnHandEnhanceStarted()
    {
        GameManager.Instance.RegisterEvent(GameState.Roll, null, DisableDiceInteraction);
    }

    protected override void OnHandEnhanceCompleted()
    {
        GameManager.Instance.UnregisterEvent(GameState.Roll, null, DisableDiceInteraction);
    }

    private void DisableDiceInteraction()
    {
        IsInteractable = false;
    }
    #endregion

    public override void ShowToolTip()
    {
        string name = availityDiceSO.diceName;
        string description = availityDiceSO.GetDescriptionText();
        ToolTipUI.Instance.ShowToolTip(transform, Vector2.down, name, description);
    }

    protected override void InitDiceInteractType()
    {
        base.InitDiceInteractType();

        if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            DiceInteractType = DiceInteractType.Sell;
        }
    }

    public virtual bool IsTriggered(EffectTriggerType triggerType, AvailityDiceContext context)
    {
        return IsEnabled && availityDiceSO.IsTriggered(triggerType, new(this, context.playDice, context.handSO));
    }

    public virtual void TriggerEffect()
    {
        if (availityDiceSO == null || !IsEnabled) return;

        availityDiceSO.TriggerEffect(new(this));
    }
}
