using System;
using System.Collections.Generic;

/// <summary>
/// 리더보드 UI와 관련된 이벤트를 정의하는 클래스
/// </summary>
public static class LeaderboardUIEvents
{
    public static event Action OnShowRequested;
    public static event Action<double> OnMyBestScoreUpdated;
    public static event Action<List<LeaderboardEntry>> OnLeaderboardUpdated;
    public static void TriggerOnShowRequested() => OnShowRequested?.Invoke();
    public static void TriggerOnMyBestScoreUpdated(double score) => OnMyBestScoreUpdated?.Invoke(score);
    public static void TriggerOnLeaderboardUpdated(List<LeaderboardEntry> leaderboard) => OnLeaderboardUpdated?.Invoke(leaderboard);
}