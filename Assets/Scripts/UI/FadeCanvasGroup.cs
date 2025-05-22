using System;
using DG.Tweening;
using UnityEngine;

public class FadeCanvasGroup : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn(float duration, Action onComplete = null)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, duration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void FadeOut(float duration, Action onComplete = null)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0f, duration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
