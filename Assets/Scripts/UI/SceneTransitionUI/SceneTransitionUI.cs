using System;
using UnityEngine;

public class SceneTransitionUI : Singleton<SceneTransitionUI>
{
    [SerializeField] private TransitionUI transitionUI;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(float duration, Action onComplete = null)
    {
        gameObject.SetActive(true);
        transitionUI.FadeIn(duration, () =>
        {
            onComplete?.Invoke();
        });
    }

    public void Hide(float duration, Action onComplete = null)
    {
        transitionUI.FadeOut(duration, () =>
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }
}