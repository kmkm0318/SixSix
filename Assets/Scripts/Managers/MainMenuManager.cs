using Firebase.Extensions;
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

        ShowMyBestScore();
        ShowLeaderboard();
        ShowLeaderboardUI();
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
    /// 내 최고 점수를 파이어베이스에서 가져와 리더보드 UI에 표시 요청
    /// </summary>
    private async void ShowMyBestScore()
    {
        await FirebaseManager.Instance.GetMyBestScore().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                double bestScore = task.Result;
                LeaderboardUIEvents.TriggerOnMyBestScoreUpdated(bestScore);
            }
            else
            {
                Debug.LogError("Show My Best Score Failed: " + task.Exception);
            }
        });
    }

    /// <summary>
    /// 리더보드 데이터를 파이어베이스에서 가져와 리더보드 UI에 표시 요청
    /// </summary>
    private async void ShowLeaderboard()
    {
        await FirebaseManager.Instance.GetTopScores().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                var leaderboard = task.Result;
                LeaderboardUIEvents.TriggerOnLeaderboardUpdated(leaderboard);
            }
            else
            {
                Debug.LogError("Show Lederboard Failed: " + task.Exception);
            }
        });
    }

    /// <summary>
    /// 리더보드 UI 표시 요청
    /// </summary>
    private void ShowLeaderboardUI()
    {
        LeaderboardUIEvents.TriggerOnShowRequested();
    }
}