using UnityEngine;

public class ShopDiceIcon : UIMouseHandler
{
    private AbilityDiceSO abilityDiceSO;
    private GambleDiceSO gambleDiceSO;

    private bool isShowToolTip = false;

    private void Start()
    {
        OnPointerEntered += ShowToolTip;
        OnPointerExited += HideToolTip;
    }

    public void Init(AbilityDiceSO abilityDiceSO)
    {
        this.abilityDiceSO = abilityDiceSO;
        SetImage();
    }

    public void Init(GambleDiceSO gambleDiceSO)
    {
        this.gambleDiceSO = gambleDiceSO;
        SetImage();
    }

    public void SetImage()
    {
        var diceSpriteListSO = DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO;

        if (abilityDiceSO != null)
        {
            Image.material = new(Image.material);
            abilityDiceSO.shaderDataSO.SetMaterialProperties(Image.material);
            Image.sprite = diceSpriteListSO.spriteList[abilityDiceSO.MaxDiceValue - 1];
        }
        else if (gambleDiceSO != null)
        {
            Image.material = new(Image.material);
            gambleDiceSO.shaderDataSO.SetMaterialProperties(Image.material);
            Image.sprite = diceSpriteListSO.spriteList[gambleDiceSO.MaxDiceValue - 1];
        }
    }

    private void ShowToolTip()
    {
        if (isShowToolTip) return;
        if (abilityDiceSO != null)
        {
            isShowToolTip = true;
            ToolTipUIEvents.TriggerOnToolTipShowRequested(transform, Vector2.left, abilityDiceSO.DiceName, abilityDiceSO.GetDescriptionText(), ToolTipTag.AbilityDice, abilityDiceSO.rarity);
        }
        else if (gambleDiceSO != null)
        {
            isShowToolTip = true;
            ToolTipUIEvents.TriggerOnToolTipShowRequested(transform, Vector2.left, gambleDiceSO.DiceName, gambleDiceSO.GetDescriptionText(), ToolTipTag.GambleDice);
        }
    }

    private void HideToolTip()
    {
        if (!isShowToolTip) return;
        isShowToolTip = false;

        ToolTipUIEvents.TriggerOnToolTipHideRequested();
    }
}