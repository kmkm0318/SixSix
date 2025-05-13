using UnityEngine;

public abstract class GambleEffectSO : ScriptableObject
{
    public abstract void TriggerEffect(GambleDice gambleDice);
    public abstract string GetEffectDescription(GambleDiceSO availityDiceSO);
}