using UnityEngine;
using UnityEngine.Localization;

public abstract class QuestData : ScriptableObject
{
    [SerializeField] protected int questID;
    [SerializeField] protected LocalizedString questName;
    [SerializeField] protected LocalizedString questDescription;
    [SerializeField] protected int chipReward;

    public int QuestID => questID;
    public int ChipReward => chipReward;
    public abstract QuestTriggerType TriggerType { get; }
    public virtual string GetName() => questName.GetLocalizedString();
    public abstract string GetDescription(double progress = 0f);
    public abstract bool IsCleared(double progress = 0f);
    public abstract void TriggerQuest(object value, ref double progress);
}

public enum QuestTriggerType
{
    PlayDiceTrigger,
    HandPlay,
    RoundClear,
}