using UnityEngine;

[CreateAssetMenu(fileName = "AvailityDiceSO", menuName = "Scriptable Objects/AvailityDiceSO")]
public class AvailityDiceSO : ScriptableObject
{
    [Header("Dice Info")]
    public string diceName;
    public AvailityDiceRarity rarity;
    public int price;
    public int SellPrice => price / 2;
    public DiceFaceSpriteListSO diceFaceSpriteListSO;
    public int maxFaceValue;
    public int MaxFaceValue => Mathf.Min(maxFaceValue, diceFaceSpriteListSO.DiceFaceCount);


    [Header("Dice Trigger, Effect")]
    public AvailityTriggerSO availityTrigger;
    public AvailityEffectSO availityEffect;

    #region GetDescriptionText
    public string GetDescriptionText()
    {
        return availityTrigger.GetTriggerDescription(this) + "\n" + availityEffect.GetEffectDescription(this);
    }

    #endregion
}

public enum AvailityDiceRarity
{
    Normal,
    Rare,
    Epic,
    Legendary,
}