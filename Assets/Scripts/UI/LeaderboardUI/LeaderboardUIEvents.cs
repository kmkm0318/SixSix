using System;
using System.Collections.Generic;

public static class LeaderboardUIEvents
{
    public static event Action<double> OnMyBestScoreUpdated;
    public static event Action<List<LeaderboardEntry>> OnLeaderboardUpdated;
    public static void TriggerOnMyBestScoreUpdated(double score) => OnMyBestScoreUpdated?.Invoke(score);
    public static void TriggerOnLeaderboardUpdated(List<LeaderboardEntry> leaderboard) => OnLeaderboardUpdated?.Invoke(leaderboard);
}