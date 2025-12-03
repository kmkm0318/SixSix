using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Pool;

/// <summary>
/// 파이어베이스에서 가져온 리더보드 데이터를 보여주는 UI 클래스
/// </summary>
public class LeaderboardUI : BaseUI
{
    [SerializeField] private LeaderboardEntryUI _myBestEntryUI;
    [SerializeField] private LeaderboardEntryUI _leaderboardEntryPrefab;
    [SerializeField] private Transform _leaderboardEntryParent;

    private ObjectPool<LeaderboardEntryUI> _entryPool;

    private void Awake()
    {
        InitPool();

        RegisterEvents();

        gameObject.SetActive(false);
    }

    private void InitPool()
    {
        _entryPool = new(
            () => Instantiate(_leaderboardEntryPrefab, _leaderboardEntryParent),
            (entry) =>
            {
                entry.gameObject.SetActive(true);
                entry.transform.SetAsLastSibling();
            },
            (entry) => entry.gameObject.SetActive(false),
            (entry) => Destroy(entry.gameObject),

            false,
            10,
            100
        );
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        LeaderboardUIEvents.OnShowRequested += OnShowRequested;
        LeaderboardUIEvents.OnMyBestScoreUpdated += OnMyBestScoreUpdated;
        LeaderboardUIEvents.OnLeaderboardUpdated += OnLeaderboardUpdated;
    }

    private void UnregisterEvents()
    {
        LeaderboardUIEvents.OnShowRequested -= OnShowRequested;
        LeaderboardUIEvents.OnMyBestScoreUpdated -= OnMyBestScoreUpdated;
        LeaderboardUIEvents.OnLeaderboardUpdated -= OnLeaderboardUpdated;
    }
    #endregion

    #region 이벤트 핸들러
    private void OnShowRequested()
    {
        Show();
    }

    private void OnMyBestScoreUpdated(double score)
    {
        _myBestEntryUI.Init("You", score);
    }

    private void OnLeaderboardUpdated(List<LeaderboardEntry> list)
    {
        foreach (Transform child in _leaderboardEntryParent)
        {
            if (child.TryGetComponent<LeaderboardEntryUI>(out var entryUI))
            {
                _entryPool.Release(entryUI);
            }
        }

        foreach (var entry in list)
        {
            var entryUI = _entryPool.Get();
            entryUI.Init(entry.playerName, entry.score);
        }
    }
    #endregion
}
