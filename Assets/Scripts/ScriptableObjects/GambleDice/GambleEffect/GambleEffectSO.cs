using UnityEngine;
using UnityEngine.Localization;

public abstract class GambleEffectSO : ScriptableObject
{
    [SerializeField] protected LocalizedString effectDescription;

    public abstract void TriggerEffect(GambleDice gambleDice);
    public virtual string GetEffectDescription(GambleDiceSO gambleDiceSO)
    {
        return effectDescription.GetLocalizedString();
    }
}