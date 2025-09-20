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
    public DiceSpriteListSO diceSpriteListSO;
    public ShaderDataSO shaderDataSO;
    public int maxDiceValue;
    public int MaxDiceValue => Mathf.Min(maxDiceValue, diceSpriteListSO.DiceFaceCount);


    [Header("Dice Trigger, Effect")]
    public AbilityTriggerSO abilityTrigger;
    public AbilityEffectSO abilityEffect;

    public void Init()
    {
        abilityEffect = Instantiate(abilityEffect);
    }

    public bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        return abilityTrigger.IsTriggered(triggerType, context);
    }

    public virtual void TriggerEffect(AbilityDiceContext context)
    {
        abilityEffect.TriggerEffect(context);
    }

    public virtual string GetDescriptionText()
    {
        return abilityTrigger.GetTriggerDescription(this) + "\n" + abilityEffect.GetEffectDescription(this);
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