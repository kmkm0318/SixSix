using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "GambleDiceSO", menuName = "Scriptable Objects/GambleDiceSO")]
public class GambleDiceSO : ScriptableObject
{
    [Header("Dice Info")]
    public LocalizedString diceNameLocalized;
    public string DiceName => diceNameLocalized.GetLocalizedString();
    public int price;
    public int SellPrice => price / 2;
    public ShaderDataSO shaderDataSO;
    public int maxDiceValue;
    public int MaxDiceValue => Mathf.Min(maxDiceValue, DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO.DiceFaceCount);


    [Header("Dice Trigger, Effect")]
    public GambleTriggerSO gambleTriggerSO;
    public GambleEffectSO gambleEffectSO;

    public bool IsTriggered(GambleDice gambleDice)
    {
        return gambleTriggerSO.IsTriggered(gambleDice);
    }

    public void TriggerEffect(GambleDice gambleDice)
    {
        gambleEffectSO.TriggerEffect(gambleDice);
    }

    public string GetDescriptionText()
    {
        string description = string.Empty;
        if (gambleTriggerSO != null)
        {
            description += gambleTriggerSO.GetTriggerDescription(this);
        }
        if (description != string.Empty)
        {
            description += "\n";
        }
        if (gambleEffectSO != null)
        {
            description += gambleEffectSO.GetEffectDescription(this);
        }
        return description;
    }
}