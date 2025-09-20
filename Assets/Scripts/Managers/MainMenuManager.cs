using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGMType.MainMenuScene);
    }
}