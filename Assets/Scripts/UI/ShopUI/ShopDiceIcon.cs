using UnityEngine;

public class ShopDiceIcon : UIMouseEnterExit
{
    private AvailityDiceSO availityDiceSO;

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

    public void SetImage()
    {
        if (availityDiceSO == null) return;
        Image.material = new(Image.material);
        availityDiceSO.shaderDataSO.SetMaterialProperties(Image.material);
        Image.sprite = availityDiceSO.diceSpriteListSO.spriteList[availityDiceSO.MaxDiceValue - 1];
    }

    private void ShowToolTip()
    {
        if (ToolTipUI.Instance == null) return;
        if (isShowToolTip) return;
        if (availityDiceSO != null)
        {
            isShowToolTip = true;
            ToolTipUI.Instance.ShowToolTip(transform, Vector2.left, availityDiceSO.diceName, availityDiceSO.GetDescriptionText());
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