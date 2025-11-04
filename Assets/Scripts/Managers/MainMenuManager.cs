using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlayBGM(BGMType.MainMenuScene);
        StartUIEvents.OnPlayerStatSelected += OnPlayerStatSelected;
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
}