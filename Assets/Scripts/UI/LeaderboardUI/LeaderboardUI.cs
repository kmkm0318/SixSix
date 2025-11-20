using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private LeaderboardEntryUI _myBestEntryUI;
    [SerializeField] private LeaderboardEntryUI _leaderboardEntryPrefab;
    [SerializeField] private Transform _leaderboardEntryParent;

    private ObjectPool<LeaderboardEntryUI> _entryPool;

    private void Awake()
    {
        LeaderboardUIEvents.OnMyBestScoreUpdated += OnMyBestScoreUpdated;
        LeaderboardUIEvents.OnLeaderboardUpdated += OnLeaderboardUpdated;

        InitPool();
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
        LeaderboardUIEvents.OnMyBestScoreUpdated -= OnMyBestScoreUpdated;
        LeaderboardUIEvents.OnLeaderboardUpdated -= OnLeaderboardUpdated;
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
}
