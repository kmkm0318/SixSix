using UnityEngine;

public abstract class AvailityTriggerSO : ScriptableObject
{
    public abstract bool IsTriggered(AvailityDiceContext context);
    public abstract string GetTriggerDescription(AvailityDiceSO availityDiceSO);
}