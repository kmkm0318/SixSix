using UnityEngine;

public class AbilityDice : Dice
{
    private AbilityDiceSO abilityDiceSO;
    private AbilityDiceContext previousContext;

    public AbilityDiceSO AbilityDiceSO => abilityDiceSO;

    //같은 Effect SO를 사용하는 각 Ability Dice에서 서로 다른 Effect 값을 가질 수 있도록 함
    public int EffectValue { get; private set; } = 0;

    public void Init(AbilityDiceSO abilityDiceSO, Playboard playboard)
    {
        this.abilityDiceSO = abilityDiceSO;

        var diceSpriteListSO = DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO;
        base.Init(abilityDiceSO.MaxDiceValue, diceSpriteListSO, abilityDiceSO.shaderDataSO, playboard);
    }

    #region Events
    override protected void OnRollCompleted()
    {
        base.OnRollCompleted();

        if (IsInteractable && DiceInteractionType == DiceInteractionType.Keep && DiceManager.Instance.IsAbilityDiceAutoKeep && DiceManager.Instance.IsKeepable)
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
        DiceInteractionType = DiceInteractionType.Sell;
    }

    protected override void OnShopEnded()
    {
        IsInteractable = false;
        DiceInteractionType = DiceInteractionType.Keep;
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
        string description = GetDescriptionText();
        ToolTipUIEvents.TriggerOnToolTipShowRequested(transform, Vector2.down, name, description, ToolTipTag.AbilityDice, abilityDiceSO.rarity);
    }

    public override void ShowInteractionInfo()
    {
        InteractionInfoUIEvents.TriggerOnShowInteractionInfoUI(transform, DiceInteractionType, abilityDiceSO.SellPrice);
    }

    protected override void InitDiceInteractType()
    {
        base.InitDiceInteractType();

        if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            DiceInteractionType = DiceInteractionType.Sell;
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

    public string GetDescriptionText()
    {
        return abilityDiceSO.GetDescriptionText(EffectValue);
    }

    public void IncreaseEffectValue(int amount)
    {
        EffectValue += amount;
    }
}
