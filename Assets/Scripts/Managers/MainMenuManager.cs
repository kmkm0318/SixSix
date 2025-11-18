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
        double bestScore = await FirebaseManager.Instance.GetMyBestScore();
        Debug.Log("My Best Score: " + bestScore);
    }

    private async void ShowLeaderboard()
    {
        var leaderboard = await FirebaseManager.Instance.GetTopScores();
        Debug.Log("Leaderboard:");
        foreach (var entry in leaderboard)
        {
            Debug.Log($"Player: {entry.playerName}, Score: {entry.score}");
        }
    }
}