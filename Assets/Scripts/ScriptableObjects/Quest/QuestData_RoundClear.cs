using UnityEngine;

[CreateAssetMenu(fileName = "QuestData_RoundClear", menuName = "Scriptable Objects/Quest/QuestData_RoundClear", order = 0)]
public class QuestData_RoundClear : QuestData
{
    [SerializeField] private int targetRound;
    [SerializeField] private int targetCount;

    public override QuestTriggerType TriggerType => QuestTriggerType.RoundClear;

    public override string GetDescription(double progress = 0)
    {
        return questDescription.GetLocalizedString(targetRound, targetCount, (int)progress);
    }

    public override bool IsCleared(double progress = 0)
    {
        return progress >= targetCount;
    }

    public override void TriggerQuest(object value, ref double progress)
    {
        if (value is int clearRound && targetRound == clearRound)
        {
            progress++;
        }
    }
}