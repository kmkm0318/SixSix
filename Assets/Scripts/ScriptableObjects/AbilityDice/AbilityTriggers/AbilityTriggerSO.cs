using UnityEngine;
using UnityEngine.Localization;

public abstract class AbilityTriggerSO : ScriptableObject
{
    [SerializeField] protected EffectTriggerType triggerType;
    [SerializeField] protected LocalizedString triggerDescription;
    public EffectTriggerType TriggerType => triggerType;

    public abstract bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context);
    public virtual string GetTriggerDescription(AbilityDiceSO abilityDiceSO)
    {
        return triggerDescription.GetLocalizedString();
    }
}

public enum EffectTriggerType
{
    None,
    PlayDiceApplied,
    HandApplied,
    RoundStarted,
    RoundCleared,
    ShopStarted,
    ShopEnded,
    PlayStarted,
    PlayEnded,
    RollStarted,
    RollEnded,
}