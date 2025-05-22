using System;
using System.Collections;
using UnityEngine;

public class LoadingManager : Singleton<LoadingManager>
{
    [SerializeField] private float waitDuration = 2f;
    [SerializeField] private float loadingDuration = 2f;

    protected override void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }

    public void StartLoading(Action onComplete = null)
    {
        StartCoroutine(LoadingCoroutine(onComplete));
    }

    private IEnumerator LoadingCoroutine(Action onComplete)
    {
        LoadingCanvas.Instance.Show(0f);
        yield return new WaitForSeconds(waitDuration);
        LoadingCanvas.Instance.Hide(loadingDuration, () =>
        {
            onComplete?.Invoke();
        });
        yield break;
    }
}