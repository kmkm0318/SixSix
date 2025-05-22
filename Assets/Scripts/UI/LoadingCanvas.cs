using System;
using UnityEngine;

public class LoadingCanvas : Singleton<LoadingCanvas>
{
    [SerializeField] private FadeCanvasGroup fadePanel;

    override protected void Awake()
    {
        dontDestroyOnLoad = true;
        base.Awake();
    }

    public void Show(float duration, Action onComplete = null)
    {
        gameObject.SetActive(true);
        fadePanel.FadeIn(duration, () =>
        {
            onComplete?.Invoke();
        });
    }

    public void Hide(float duration, Action onComplete = null)
    {
        fadePanel.FadeOut(duration, () =>
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }
}