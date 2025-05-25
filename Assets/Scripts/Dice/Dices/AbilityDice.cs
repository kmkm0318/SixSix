using UnityEngine;

public class AbilityDice : Dice
{
    private AbilityDiceSO abilityDiceSO;
    public AbilityDiceSO AbilityDiceSO => abilityDiceSO;
    public int SellPrice => abilityDiceSO.SellPrice;

    public void Init(AbilityDiceSO abilityDiceSO, Playboard playboard)
    {
        base.Init(abilityDiceSO.MaxDiceValue, abilityDiceSO.diceSpriteListSO, abilityDiceSO.shaderDataSO, playboard);

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
        ToolTipUI.Instance.ShowToolTip(transform, Vector2.down, name, description, ToolTipTag.Ability_Dice, abilityDiceSO.rarity);
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
        return IsEnabled && abilityDiceSO.IsTriggered(triggerType, new(this, context.playDice, context.handSO));
    }

    public virtual void TriggerEffect()
    {
        if (abilityDiceSO == null || !IsEnabled) return;

        abilityDiceSO.TriggerEffect(new(this));
    }
}
