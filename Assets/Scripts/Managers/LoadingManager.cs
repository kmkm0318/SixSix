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
        yield return new WaitUntil(() => FirebaseManager.Instance != null && FirebaseManager.Instance.IsInitialized);
        SceneTransitionManager.Instance.ChangeScene(SceneType.MainMenu);
    }
}