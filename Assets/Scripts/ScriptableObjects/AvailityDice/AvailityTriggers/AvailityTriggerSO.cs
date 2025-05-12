using UnityEngine;

public abstract class AvailityTriggerSO : ScriptableObject
{
    [SerializeField] private AvailityTriggerType triggerType;
    public AvailityTriggerType TriggerType => triggerType;

    public abstract bool IsTriggered(AvailityTriggerType triggerType, AvailityDiceContext context);
    public abstract string GetTriggerDescription(AvailityDiceSO availityDiceSO);
}

public enum AvailityTriggerType
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