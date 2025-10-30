using UnityEngine;
using UnityEngine.Localization;

public abstract class QuestData : ScriptableObject
{
    [SerializeField] protected int questID;
    [SerializeField] protected LocalizedString questName;
    [SerializeField] protected LocalizedString questDescription;

    public int QuestID => questID;
    public virtual string GetName() => questName.GetLocalizedString();
    public abstract string GetDescription(double progress = 0f);
    public abstract bool IsCleared(double progress = 0f);
}