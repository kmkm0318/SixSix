using UnityEngine;

public abstract class GambleTriggerSO : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] private string description;

    public abstract bool IsTriggered(GambleDice gambleDice);
    public virtual string GetTriggerDescription(GambleDiceSO gambleDiceSO)
    {
        return description;
    }
}