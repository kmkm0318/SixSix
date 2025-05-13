using UnityEngine;

[CreateAssetMenu(fileName = "GambleDiceSO", menuName = "Scriptable Objects/GambleDiceSO")]
public class GambleDiceSO : ScriptableObject
{
    [Header("Dice Info")]
    public string diceName;
    public int price;
    public int SellPrice => price / 2;
    public DiceSpriteListSO diceSpriteListSO;
    public DiceMaterialSO diceMaterialSO;
    public int maxDiceValue;
    public int MaxDiceValue => Mathf.Min(maxDiceValue, diceSpriteListSO.DiceFaceCount);

    [Header("Dice Effect")]
    public GambleEffectSO gambleEffect;

    public void TriggerEffect(GambleDice gambleDice)
    {
        gambleEffect.TriggerEffect(gambleDice);
    }

    public string GetDescriptionText()
    {
        return gambleEffect.GetEffectDescription(this);
    }
}