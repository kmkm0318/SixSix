using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "AbilityDiceSO", menuName = "Scriptable Objects/AbilityDiceSO")]
public class AbilityDiceSO : ScriptableObject
{
    [Header("Dice Info")]
    public int abilityDiceID;
    public LocalizedString diceNameLocalized;
    public string DiceName => diceNameLocalized.GetLocalizedString();
    public AbilityDiceRarity rarity;
    public AbilityDiceAutoKeepType autoKeepType;
    public int price;
    public int SellPrice => price / 2;
    public ShaderDataSO shaderDataSO;
    public int maxDiceValue;
    public int MaxDiceValue => Mathf.Min(maxDiceValue, DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO.DiceFaceCount);


    [Header("Dice Trigger, Effect, Unlock")]
    public AbilityTriggerSO abilityTrigger;
    public AbilityEffectSO abilityEffect;
    public AbilityDiceUnlockSO abilityUnlock;

    public bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        return abilityTrigger.IsTriggered(triggerType, context);
    }

    public void TriggerEffect(AbilityDiceContext context)
    {
        abilityEffect.TriggerEffect(context);
    }

    public string GetDescriptionText(int effectValue = 0)
    {
        return abilityTrigger.GetTriggerDescription(this) + "\n" + abilityEffect.GetEffectDescription(this, effectValue);
    }

    public bool IsUnlcoked()
    {
        return abilityUnlock.IsUnlocked();
    }

    public string GetUnlockDescriptionText()
    {
        return abilityUnlock.GetDescriptionText();
    }
}

public enum AbilityDiceRarity
{
    Normal,
    Rare,
    Epic,
    Legendary,
}

public enum AbilityDiceAutoKeepType
{
    High,
    Low,
}