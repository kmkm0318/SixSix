using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] private AnimatedText _questDescriptionText;
    [SerializeField] private ButtonPanel _clearButton;

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
        _clearButton.gameObject.SetActive(activeQuest.isCleared && !activeQuest.isRewarded);
    }
}