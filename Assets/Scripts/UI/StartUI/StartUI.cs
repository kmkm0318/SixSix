using UnityEngine;
using UnityEngine.Pool;

public class StartUI : BaseUI
{
    [SerializeField] private PlayerStatSelectUI _playerStatSelectButtonPrefab;
    [SerializeField] private Transform _playerStatSelectButtonParent;
    [SerializeField] private ButtonPanel _closeButton;

    private ObjectPool<PlayerStatSelectUI> _buttonPool;

    private void Start()
    {
        InitPool();
        InitButtons();
        _closeButton.OnClick += () => Hide();
        StartUIEvents.OnStartUIButtonClicked += OnStartUIButtonClicked;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StartUIEvents.OnStartUIButtonClicked -= OnStartUIButtonClicked;
    }

    private void OnStartUIButtonClicked()
    {
        Show();
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

    private void OnSelected(PlayerStatSelectUI button)
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
}