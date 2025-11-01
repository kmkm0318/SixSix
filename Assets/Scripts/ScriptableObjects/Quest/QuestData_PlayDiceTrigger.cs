using UnityEngine;

[CreateAssetMenu(fileName = "QuestData_PlayDiceTrigger", menuName = "Scriptable Objects/Quest/QuestData_PlayDiceTrigger", order = 0)]
public class QuestData_PlayDiceTrigger : QuestData
{
    [SerializeField] private int targetPlayDiceValue;
    [SerializeField] private int targetCount;

    public override QuestTriggerType TriggerType => QuestTriggerType.PlayDiceTrigger;

    public override string GetDescription(double progress = 0)
    {
        return questDescription.GetLocalizedString(targetPlayDiceValue, targetCount, (int)progress);
    }

    public override bool IsCleared(double progress = 0)
    {
        return progress >= targetCount;
    }

    public override void TriggerQuest(object value, ref double progress)
    {
        if (value is int playDiceValue && targetPlayDiceValue == playDiceValue)
        {
            progress++;
        }
    }
}