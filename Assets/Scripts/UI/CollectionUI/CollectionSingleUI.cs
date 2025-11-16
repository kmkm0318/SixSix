using UnityEngine;

public class CollectionSingleUI : UIFocusHandler
{
    public void Init(AbilityDiceSO abilityDiceSO)
    {
        var defaultDiceSpriteList = DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO.spriteList;

        string descriptionText = abilityDiceSO.IsUnlcoked() ? abilityDiceSO.GetDescriptionText() : abilityDiceSO.abilityUnlock.GetDescriptionText();
        Color color = abilityDiceSO.IsUnlcoked() ? Color.white : new Color(1f, 1f, 1f, 0.5f);

        OnPointerEntered += () =>
        {
            ToolTipUIEvents.TriggerOnToolTipShowRequested(
                RectTransform,
                Vector2.left,
                abilityDiceSO.DiceName,
                descriptionText,
                ToolTipTag.AbilityDice,
                abilityDiceSO.rarity
            );
        };

        OnPointerExited += () =>
        {
            ToolTipUIEvents.TriggerOnToolTipHideRequested(RectTransform);
        };

        Image.sprite = defaultDiceSpriteList[abilityDiceSO.MaxDiceValue - 1];
        Image.material = new(Image.material);
        abilityDiceSO.shaderDataSO.SetMaterialProperties(Image.material);
        Image.color = color;
    }
}