using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class StartUI : MonoBehaviour
{
    [SerializeField] private RectTransform _startPanel;
    [SerializeField] private Vector3 _hidePos;
    [SerializeField] private PlayerStatSelectButton _playerStatSelectButtonPrefab;
    [SerializeField] private Transform _playerStatSelectButtonParent;
    [SerializeField] private FadeCanvasGroup _fadeCanvasGroup;
    [SerializeField] private ButtonPanel _closeButton;

    private ObjectPool<PlayerStatSelectButton> _buttonPool;

    private void Start()
    {
        InitPool();
        InitButtons();
        StartUIEvents.OnStartUIButtonClicked += Show;
        _closeButton.OnClick += Hide;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StartUIEvents.OnStartUIButtonClicked -= Show;
    }

    private void InitPool()
    {
        _buttonPool = new(
            () => Instantiate(_playerStatSelectButtonPrefab, _playerStatSelectButtonParent),
            (button) =>
            {
                button.gameObject.SetActive(true);
                button.transform.SetAsLastSibling();
            },
            (button) => button.gameObject.SetActive(false),
            (button) => Destroy(button.gameObject)
        );
    }

    private void InitButtons()
    {
        var playerStatSOs = DataContainer.Instance.PlayerStatListSO.playerStatSOs;
        var achievedPlayerStatIDs = PlayerDataManager.Instance.PlayerData.achievedPlayerStatIDs;

        foreach (var statSO in playerStatSOs)
        {
            var button = _buttonPool.Get();
            bool isAchieved = achievedPlayerStatIDs.Contains(statSO.id);

            button.Init(statSO, isAchieved);
            button.OnSelected += OnSelected;
        }
    }

    private void OnSelected(PlayerStatSelectButton button)
    {
        if (button.IsAchieved)
        {
            var statSO = button.PlayerStatSO;
            StartUIEvents.TriggerOnPlayerStatSelected(statSO);
        }
        else
        {
            var statSO = button.PlayerStatSO;

            if (!PlayerDataManager.Instance.TrySubtractChip(statSO.price)) return;

            if (!PlayerDataManager.Instance.TryAddAchievedPlayerStatIDs(statSO.id)) return;

            button.SetIsAchieved(true);
        }
    }

    #region ShowHide
    private void Show()
    {
        gameObject.SetActive(true);

        _startPanel
            .DOAnchorPos(Vector3.zero, AnimationFunction.DefaultDuration)
            .From(_hidePos)
            .SetEase(Ease.InOutBack);

        _fadeCanvasGroup.FadeIn(AnimationFunction.DefaultDuration);
    }

    private void Hide()
    {
        _startPanel
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