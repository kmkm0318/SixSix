using UnityEngine;

public class ShopDiceIcon : UIMouseHandler
{
    private AvailityDiceSO availityDiceSO;
    private GambleDiceSO gambleDiceSO;

    private bool isShowToolTip = false;

    private void Start()
    {
        OnPointerEntered += ShowToolTip;
        OnPointerExited += HideToolTip;
    }

    public void Init(AvailityDiceSO availityDiceSO)
    {
        this.availityDiceSO = availityDiceSO;
        SetImage();
    }

    public void Init(GambleDiceSO gambleDiceSO)
    {
        this.gambleDiceSO = gambleDiceSO;
        SetImage();
    }

    public void SetImage()
    {
        if (availityDiceSO != null)
        {
            Image.material = new(Image.material);
            availityDiceSO.shaderDataSO.SetMaterialProperties(Image.material);
            Image.sprite = availityDiceSO.diceSpriteListSO.spriteList[availityDiceSO.MaxDiceValue - 1];
        }
        else if (gambleDiceSO != null)
        {
            Image.material = new(Image.material);
            gambleDiceSO.shaderDataSO.SetMaterialProperties(Image.material);
            Image.sprite = gambleDiceSO.diceSpriteListSO.spriteList[gambleDiceSO.MaxDiceValue - 1];
        }
    }

    private void ShowToolTip()
    {
        if (ToolTipUI.Instance == null) return;
        if (isShowToolTip) return;
        if (availityDiceSO != null)
        {
            isShowToolTip = true;
            ToolTipUI.Instance.ShowToolTip(transform, Vector2.left, availityDiceSO.diceName, availityDiceSO.GetDescriptionText(), ToolTipTag.Availity_Dice, availityDiceSO.rarity);
        }
        else if (gambleDiceSO != null)
        {
            isShowToolTip = true;
            ToolTipUI.Instance.ShowToolTip(transform, Vector2.left, gambleDiceSO.diceName, gambleDiceSO.GetDescriptionText(), ToolTipTag.Gamble_Dice);
        }
    }

    private void HideToolTip()
    {
        if (ToolTipUI.Instance == null) return;
        if (!isShowToolTip) return;
        isShowToolTip = false;

        ToolTipUI.Instance.HideToolTip();
    }
}