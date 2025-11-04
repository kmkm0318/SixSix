using UnityEngine;
using UnityEngine.Localization;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] private AnimatedText _questDescriptionText;
    [SerializeField] private ButtonPanel _clearButton;
    [SerializeField] private AnimatedText _chipRewardText;
    [SerializeField] private LocalizedString _chipString;
    [SerializeField] private LocalizedString _chipRewardedString;

    private ActiveQuest _activeQuest;

    private void Awake()
    {
        _clearButton.OnClick += OnClick;
    }

    private void OnClick()
    {
        if (_activeQuest == null) return;

        QuestManager.Instance.GetQuestReward(_activeQuest);

        UpdateItem(_activeQuest);
    }

    public void UpdateItem(ActiveQuest activeQuest)
    {
        _activeQuest = activeQuest;

        _questDescriptionText.SetText(activeQuest.questData.GetDescription(activeQuest.progress));
        _clearButton.SetInteractable(activeQuest.isCleared && !activeQuest.isRewarded);

        _chipRewardText.SetText(
            activeQuest.isRewarded ?
            _chipRewardedString.GetLocalizedString() :
            _chipString.GetLocalizedString(activeQuest.questData.ChipReward.ToString("N0")));

        _chipRewardText.TMP_Text.color = activeQuest.isCleared ? Color.white : Color.gray;
    }
}