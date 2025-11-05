using System;
using UnityEngine;
using UnityEngine.Localization;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] private AnimatedText _questDescriptionText;
    [SerializeField] private ButtonPanel _clearButton;
    [SerializeField] private AnimatedText _chipRewardText;
    [SerializeField] private LocalizedString _chipString;

    private ActiveQuest _activeQuest;

    public event Action<QuestItemUI> OnRewarded;

    private void Awake()
    {
        _clearButton.OnClick += OnClick;
    }

    private void OnClick()
    {
        if (_activeQuest == null) return;

        QuestManager.Instance.GetQuestReward(_activeQuest);

        OnRewarded?.Invoke(this);
    }

    public void UpdateItem(ActiveQuest activeQuest)
    {
        _activeQuest = activeQuest;

        _questDescriptionText.SetText(activeQuest.questData.GetDescription(activeQuest.progress));
        _clearButton.SetInteractable(activeQuest.isCleared && !activeQuest.isRewarded);

        _chipRewardText.SetText(_chipString.GetLocalizedString(activeQuest.questData.ChipReward.ToString("N0")));

        _chipRewardText.TMP_Text.color = activeQuest.isCleared ? Color.white : Color.gray;
    }
}