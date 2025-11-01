using UnityEngine;

[CreateAssetMenu(fileName = "QuestData_HandPlay", menuName = "Scriptable Objects/Quest/QuestData_HandPlay", order = 0)]
public class QuestData_HandPlay : QuestData
{
    [SerializeField] private HandSO targetHandSO;
    [SerializeField] private int targetCount;

    public override QuestTriggerType TriggerType => QuestTriggerType.HandPlay;

    public override string GetDescription(double progress = 0)
    {
        return questDescription.GetLocalizedString(targetHandSO.HandName, targetCount, (int)progress);
    }

    public override bool IsCleared(double progress = 0)
    {
        return progress >= targetCount;
    }

    public override void TriggerQuest(object value, ref double progress)
    {
        if (value is HandSO handSO && targetHandSO.hand == handSO.hand)
        {
            progress++;
        }
    }
}