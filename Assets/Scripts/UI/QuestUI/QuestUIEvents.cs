using System;

public static class QuestUIEvents
{
    public static event Action OnQuestButtonClicked;
    public static event Action OnRefreshButtonClicked;

    public static void TriggerOnQuestButtonClicked() => OnQuestButtonClicked?.Invoke();
    public static void TriggerOnRefreshButtonClicked() => OnRefreshButtonClicked?.Invoke();
}