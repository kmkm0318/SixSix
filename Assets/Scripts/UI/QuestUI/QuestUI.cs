using System;
using UnityEngine;
using UnityEngine.Pool;

public class QuestUI : BaseUI
{
    [SerializeField] private Transform _questItemParent;
    [SerializeField] private QuestItemUI _questItemUIPrefab;
    [SerializeField] private ButtonPanel _refreshButton;
    [SerializeField] private ButtonPanel _closeButton;

    private ObjectPool<QuestItemUI> _questItemPool;

    private void Start()
    {
        InitPool();
        _refreshButton.OnClick += OnRefreshButtonClicked;
        _closeButton.OnClick += () => Hide();
        QuestUIEvents.OnQuestButtonClicked += OnQuestButtonClicked;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        QuestUIEvents.OnQuestButtonClicked -= OnQuestButtonClicked;
    }

    private void OnQuestButtonClicked()
    {
        Show();
    }

    private void InitPool()
    {
        _questItemPool = new(
            () =>
            {
                var item = Instantiate(_questItemUIPrefab, _questItemParent);
                item.OnRewarded += (rewardedItem) => _questItemPool.Release(rewardedItem);
                return item;
            },
            (item) =>
            {
                item.gameObject.SetActive(true);
                item.transform.SetAsLastSibling();
            },
            (item) => item.gameObject.SetActive(false),
            (item) => Destroy(item.gameObject)
        );
    }

    private void OnRefreshButtonClicked()
    {
        QuestUIEvents.TriggerOnRefreshButtonClicked();
        UpdateQuestUI();
    }

    protected override void Show(Action onComplete = null)
    {
        base.Show(onComplete);
        UpdateQuestUI();
    }

    private void UpdateQuestUI()
    {
        foreach (Transform child in _questItemParent)
        {
            if (child.TryGetComponent<QuestItemUI>(out var item))
            {
                if (item.gameObject.activeSelf)
                {
                    _questItemPool.Release(item);
                }
            }
        }

        var activeQuests = QuestManager.Instance.ActiveQuests;

        foreach (var activeQuest in activeQuests)
        {
            if (activeQuest.isRewarded) continue;

            var newItem = _questItemPool.Get();
            newItem.UpdateItem(activeQuest);
        }
    }
}