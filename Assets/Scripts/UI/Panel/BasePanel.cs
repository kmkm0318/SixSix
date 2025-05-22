using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] protected Image image;
    [SerializeField] protected Shadow shadow;

    public void SetColor(Color color)
    {
        if (image != null)
        {
            image.color = color;
        }
    }

    public void Show(Vector2 startPos, Vector2 endPos, float duration, Ease easeType = Ease.OutBack, Action onComplete = null)
    {
        rectTransform.anchoredPosition = startPos;
        rectTransform
            .DOAnchorPos(endPos, duration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    public IEnumerator ShowCoroutine(Vector2 startPos, Vector2 endPos, float duration, Ease easeType = Ease.OutBack, Action onComplete = null)
    {
        rectTransform.anchoredPosition = startPos;
        yield return rectTransform
            .DOAnchorPos(endPos, duration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    public void Hide(Vector2 endPos, float duration, Ease easeType = Ease.OutBack, Action onComplete = null)
    {
        rectTransform
            .DOAnchorPos(endPos, duration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }

    public IEnumerator HideCoroutine(Vector2 endPos, float duration, Ease easeType = Ease.OutBack, Action onComplete = null)
    {
        yield return rectTransform
            .DOAnchorPos(endPos, duration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }
}