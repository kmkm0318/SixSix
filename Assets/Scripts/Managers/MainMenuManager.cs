using Firebase.Extensions;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlayBGM(BGMType.MainMenuScene);
        StartUIEvents.OnPlayerStatSelected += OnPlayerStatSelected;

        ShowMyBestScore();
        ShowLeaderboard();
    }

    private void OnDestroy()
    {
        StartUIEvents.OnPlayerStatSelected -= OnPlayerStatSelected;
    }

    private void OnPlayerStatSelected(PlayerStatSO statSO)
    {
        DataContainer.Instance.CurrentPlayerStat = statSO;
        SceneTransitionManager.Instance.ChangeScene(SceneType.Game);
    }

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
}