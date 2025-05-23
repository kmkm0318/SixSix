using UnityEngine;

public abstract class GambleEffectSO : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] protected string description;

    public abstract void TriggerEffect(GambleDice gambleDice);
    public virtual string GetEffectDescription(GambleDiceSO availityDiceSO)
    {
        return description;
    }
}