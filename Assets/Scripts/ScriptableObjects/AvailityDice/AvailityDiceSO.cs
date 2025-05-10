using UnityEngine;

[CreateAssetMenu(fileName = "AvailityDiceSO", menuName = "Scriptable Objects/AvailityDiceSO")]
public class AvailityDiceSO : ScriptableObject
{
    [Header("Dice Info")]
    public string diceName;
    public AvailityDiceRarity rarity;
    public AbailityDiceAutoKeepType autoKeepType;
    public int price;
    public int SellPrice => price / 2;
    public DiceSpriteListSO diceSpriteListSO;
    public DiceMaterialSO diceMaterialSO;
    public int maxDiceValue;
    public int MaxDiceValue => Mathf.Min(maxDiceValue, diceSpriteListSO.DiceFaceCount);


    [Header("Dice Trigger, Effect")]
    public AvailityTriggerSO availityTrigger;
    public AvailityEffectSO availityEffect;

    public void Init()
    {
        availityEffect = Instantiate(availityEffect);
    }

    public string GetDescriptionText()
    {
        return availityTrigger.GetTriggerDescription(this) + "\n" + availityEffect.GetEffectDescription(this);
    }
}

public enum AvailityDiceRarity
{
    Normal,
    Rare,
    Epic,
    Legendary,
}

public enum AbailityDiceAutoKeepType
{
    High,
    Low,
}