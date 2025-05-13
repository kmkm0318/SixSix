using UnityEngine;

public abstract class AvailityTriggerSO : ScriptableObject
{
    [SerializeField] private EffectTriggerType triggerType;
    public EffectTriggerType TriggerType => triggerType;

    public abstract bool IsTriggered(EffectTriggerType triggerType, AvailityDiceContext context);
    public abstract string GetTriggerDescription(AvailityDiceSO availityDiceSO);
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