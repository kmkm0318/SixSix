using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlayBGM(BGMType.MainMenuScene);
    }
}