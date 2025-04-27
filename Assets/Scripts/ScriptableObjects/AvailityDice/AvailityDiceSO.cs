using UnityEngine;

[CreateAssetMenu(fileName = "AvailityDiceSO", menuName = "Scriptable Objects/AvailityDiceSO")]
public class AvailityDiceSO : ScriptableObject
{
    [Header("Dice Info")]
    public string diceName;
    public int purchasePrice;
    public int sellPrice;
    public int maxFaceValue;
    public DiceFaceSpriteListSO diceFaceSpriteListSO;

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