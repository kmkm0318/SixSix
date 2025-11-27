using System.Collections;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadingCoroutine());
    }

    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitUntil(() => FirebaseManager.Instance != null && FirebaseManager.Instance._isInitialized);
        SceneTransitionManager.Instance.ChangeScene(SceneType.MainMenu);
    }
}