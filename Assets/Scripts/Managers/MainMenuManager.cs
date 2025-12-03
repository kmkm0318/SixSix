using System;
using UnityEngine;

/// <summary>
/// 메인 메뉴 씬을 관리하는 메니저 클래스
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlayBGM(BGMType.MainMenuScene);
        StartUIEvents.OnPlayerStatSelected += OnPlayerStatSelected;

        // TryShowLeaderboardUI();
    }

    private void OnDestroy()
    {
        StartUIEvents.OnPlayerStatSelected -= OnPlayerStatSelected;
    }

    /// <summary>
    /// 플레이어가 선택한 스탯으로 게임 씬으로 전환
    /// </summary>
    private void OnPlayerStatSelected(PlayerStatSO statSO)
    {
        DataContainer.Instance.CurrentPlayerStat = statSO;
        SceneTransitionManager.Instance.ChangeScene(SceneType.Game);
    }

    /// <summary>
    /// 리더보드 UI 표시 시도
    /// FireBaseManager로부터 나의 최고 기록과 리더보드를 가져옴
    /// </summary>
    private async void TryShowLeaderboardUI()
    {
        try
        {
            var myBestScore = await FirebaseManager.Instance.GetMyBestScore();
            var leaderboardData = await FirebaseManager.Instance.GetTopScores();

            LeaderboardUIEvents.TriggerOnMyBestScoreUpdated(myBestScore);
            LeaderboardUIEvents.TriggerOnLeaderboardUpdated(leaderboardData);

            LeaderboardUIEvents.TriggerOnShowRequested();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed To Load Leaderboard: {e.Message}");
        }
    }
}