using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    private const string ACTIVE_QUESTS = "ActiveQuests";

    [SerializeField] private QuestDataList questDataList;
    private List<ActiveQuest> activeQuests = new();

    public List<ActiveQuest> ActiveQuests => activeQuests;

    protected override void Awake()
    {
        base.Awake();
        LoadActiveQuests();
    }

    private void OnEnable()
    {
        QuestUIEvents.OnRefreshButtonClicked += OnRefreshButtonClicked;
    }

    private void OnDisable()
    {
        QuestUIEvents.OnRefreshButtonClicked -= OnRefreshButtonClicked;
    }

    private void OnRefreshButtonClicked()
    {
        InitActiveQuests();
        SaveActiveQuests();
    }

    #region Save/Load
    private void LoadActiveQuests()
    {
        string json = PlayerPrefs.GetString(ACTIVE_QUESTS, string.Empty);

        if (json == string.Empty)
        {
            InitActiveQuests();
            SaveActiveQuests();
            return;
        }

        var wrapper = JsonUtility.FromJson<ActiveQuestListWrapper>(json);
        activeQuests = wrapper.activeQuests;

        foreach (var quest in activeQuests)
        {
            var questData = GetQuestData(quest.questID);
            if (questData == null) continue;

            quest.questData = questData;
            quest.isCleared = questData.IsCleared(quest.progress);
        }
    }

    private void SaveActiveQuests()
    {
        ActiveQuestListWrapper wrapper = new() { activeQuests = activeQuests };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(ACTIVE_QUESTS, json);
        PlayerPrefs.Save();
    }
    #endregion

    private void InitActiveQuests(int count = 3)
    {
        activeQuests.Clear();

        var newQuestDatas = questDataList.questDatas.GetRandomElements(count);

        foreach (var questData in newQuestDatas)
        {
            var newActiveQuest = new ActiveQuest
            {
                questData = questData,
                questID = questData.QuestID
            };

            activeQuests.Add(newActiveQuest);
        }
    }

    private QuestData GetQuestData(int questID)
    {
        foreach (var questData in questDataList.questDatas)
        {
            if (questData.QuestID == questID)
            {
                return questData;
            }
        }

        return null;
    }

    public void TriggerActiveQuest(QuestTriggerType triggerType, object value)
    {
        foreach (var activeQuest in activeQuests)
        {
            if (activeQuest.isCleared) continue;
            if (activeQuest.questData.TriggerType != triggerType) continue;

            activeQuest.questData.TriggerQuest(value, ref activeQuest.progress);
            if (activeQuest.questData.IsCleared(activeQuest.progress)) activeQuest.isCleared = true;
        }

        SaveActiveQuests();
    }

    public void GetQuestReward(ActiveQuest activeQuest)
    {
        activeQuest.isRewarded = true;

        PlayerDataManager.Instance.AddChip(activeQuest.questData.ChipReward);

        SaveActiveQuests();
    }
}

[Serializable]
public class ActiveQuest
{
    public QuestData questData;
    public int questID = 0;
    public double progress = 0f;
    public bool isCleared = false;
    public bool isRewarded = false;
}