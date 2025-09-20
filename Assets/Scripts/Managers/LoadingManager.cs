using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    private void Start()
    {
        SceneTransitionManager.Instance.ChangeScene(SceneType.MainMenu);
    }
}