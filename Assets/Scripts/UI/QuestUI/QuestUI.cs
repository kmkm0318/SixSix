using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private RectTransform _questPanel;
    [SerializeField] private Vector3 _hidePos;
    [SerializeField] private Transform _questItemParent;
    [SerializeField] private QuestItemUI _questItemUIPrefab;
    [SerializeField] private FadeCanvasGroup _fadeCanvasGroup;
    [SerializeField] private ButtonPanel _refreshButton;
    [SerializeField] private ButtonPanel _closeButton;

    private ObjectPool<QuestItemUI> _questItemPool;

    private void Start()
    {
        InitPool();
        _refreshButton.OnClick += OnRefreshButtonClicked;
        _closeButton.OnClick += Hide;
        QuestUIEvents.OnQuestButtonClicked += Show;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        QuestUIEvents.OnQuestButtonClicked -= Show;
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

    private void UpdateQuestUI()
    {
        foreach (Transform child in _questItemParent)
        {
            if (child.TryGetComponent<QuestItemUI>(out var item))
            {
                _questItemPool.Release(item);
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

    #region ShowHide
    private void Show()
    {
        gameObject.SetActive(true);

        UpdateQuestUI();

        _questPanel
            .DOAnchorPos(Vector3.zero, AnimationFunction.DefaultDuration)
            .From(_hidePos)
            .SetEase(Ease.InOutBack);

        _fadeCanvasGroup.FadeIn(AnimationFunction.DefaultDuration);
    }

    private void Hide()
    {
        _questPanel
            .DOAnchorPos(_hidePos, AnimationFunction.DefaultDuration)
            .From(Vector3.zero)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });

        _fadeCanvasGroup.FadeOut(AnimationFunction.DefaultDuration);
    }
    #endregion
}