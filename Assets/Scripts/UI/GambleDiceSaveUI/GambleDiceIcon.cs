using UnityEngine;

public class GambleDiceIcon : UIMouseHandler
{
    private GambleDiceSO gambleDiceSO;

    private bool isShowToolTip = false;

    private void Start()
    {
        OnPointerEntered += ShowToolTip;
        OnPointerExited += HideToolTip;
        OnPointerClicked += () => GambleDiceSaveUI.Instance.HandleGambleDiceIconClicked(this);
        OnPointerClicked += HideToolTip;
    }

    public void Init(GambleDiceSO gambleDiceSO)
    {
        this.gambleDiceSO = gambleDiceSO;
        SetImage();
    }

    public void SetImage()
    {
        if (gambleDiceSO != null)
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
        if (gambleDiceSO != null)
        {
            isShowToolTip = true;
            ToolTipUI.Instance.ShowToolTip(RectTransform, Vector2.down, gambleDiceSO.diceName, gambleDiceSO.GetDescriptionText(), ToolTipTag.Gamble_Dice);
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