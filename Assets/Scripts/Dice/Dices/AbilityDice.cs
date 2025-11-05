using UnityEngine;

public class AbilityDice : Dice
{
    private AbilityDiceSO abilityDiceSO;
    private AbilityDiceContext previousContext;

    public AbilityDiceSO AbilityDiceSO => abilityDiceSO;
    public int SellPrice => abilityDiceSO.SellPrice;

    public void Init(AbilityDiceSO abilityDiceSO, Playboard playboard)
    {
        var diceSpriteListSO = DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO;

        base.Init(abilityDiceSO.MaxDiceValue, diceSpriteListSO, abilityDiceSO.shaderDataSO, playboard);

        this.abilityDiceSO = Instantiate(abilityDiceSO);
        this.abilityDiceSO.Init();
    }

    #region Events
    override protected void OnRollCompleted()
    {
        base.OnRollCompleted();

        if (IsInteractable && DiceInteractType == DiceInteractType.Keep && DiceManager.Instance.IsAbilityDiceAutoKeep && DiceManager.Instance.IsKeepable)
        {
            if (abilityDiceSO && abilityDiceSO.autoKeepType == AbilityDiceAutoKeepType.High)
            {
                if (DiceValue >= abilityDiceSO.MaxDiceValue)
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
        string name = abilityDiceSO.DiceName;
        string description = abilityDiceSO.GetDescriptionText();
        ToolTipUI.Instance.ShowToolTip(transform, Vector2.down, name, description, ToolTipTag.AbilityDice, abilityDiceSO.rarity);
    }

    protected override void InitDiceInteractType()
    {
        base.InitDiceInteractType();

        if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            DiceInteractType = DiceInteractType.Sell;
        }
    }

    public virtual bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        context ??= new();
        context.currentAbilityDice = this;
        return IsEnabled && context.currentAbilityDice != context.abilityDice && abilityDiceSO.IsTriggered(triggerType, context);
    }

    public virtual void TriggerEffect(AbilityDiceContext context = null)
    {
        if (abilityDiceSO == null || !IsEnabled) return;
        context ??= previousContext ?? new();
        context.currentAbilityDice = this;
        previousContext = context;
        abilityDiceSO.TriggerEffect(context);
    }
}
