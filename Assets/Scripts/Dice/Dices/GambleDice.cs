using UnityEngine;

public class GambleDice : AvailityDice
{
    private GambleDiceSO gambleDiceSO;
    private bool isTriggered;

    public void Init(GambleDiceSO gambleDiceSO, Playboard playboard)
    {
        base.Init(gambleDiceSO.MaxDiceValue, gambleDiceSO.diceSpriteListSO, gambleDiceSO.shaderDataSO, playboard);

        this.gambleDiceSO = gambleDiceSO;
        isTriggered = false;
    }

    #region Events
    protected override void OnPlayStarted()
    {
        base.OnPlayStarted();
        IsInteractable = true;
        DiceInteractType = DiceInteractType.Use;
    }

    protected override void OnRoundStarted()
    {
        base.OnRoundStarted();
        IsInteractable = true;
        DiceInteractType = DiceInteractType.Use;
    }

    override protected void OnRollCompleted()
    {
        base.OnRollCompleted();
    }

    protected override void OnShopStarted()
    {
        IsInteractable = true;
        DiceInteractType = DiceInteractType.Sell;
    }

    protected override void OnShopEnded()
    {
        IsInteractable = false;
        DiceInteractType = DiceInteractType.Use;
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

    protected override void InitDiceInteractType()
    {
        base.InitDiceInteractType();

        if (GameManager.Instance.CurrentGameState == GameState.Round)
        {
            IsInteractable = true;
        }

        if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            DiceInteractType = DiceInteractType.Sell;
        }
        else
        {
            DiceInteractType = DiceInteractType.Use;
        }
    }

    override public void TriggerEffect()
    {
        if (isTriggered) return;
        isTriggered = true;

        gambleDiceSO.TriggerEffect(this);
    }

    public override void ShowToolTip()
    {
        string name = gambleDiceSO.diceName;
        string description = gambleDiceSO.GetDescriptionText();
        ToolTipUI.Instance.ShowToolTip(transform, Vector2.down, name, description);
    }
}
