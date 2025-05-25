using UnityEngine;
using UnityEngine.Localization;

public abstract class GambleTriggerSO : ScriptableObject
{
    [SerializeField] protected LocalizedString triggerDescription;

    public abstract bool IsTriggered(GambleDice gambleDice);
    public virtual string GetTriggerDescription(GambleDiceSO gambleDiceSO)
    {
        return triggerDescription.GetLocalizedString();
    }
}