using UnityEngine;
using UnityEngine.Localization;

public abstract class AbilityDiceUnlockSO : ScriptableObject
{
    [SerializeField] protected LocalizedString unlockDescription;
    public abstract bool IsUnlocked();
    public virtual string GetDescriptionText()
    {
        return unlockDescription.GetLocalizedString();
    }
}