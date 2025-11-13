using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class BasePanel : MonoBehaviour
{
    protected RectTransform _rectTransform;
    protected Image _image;

    protected virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public void SetColor(Color color)
    {
        if (_image != null)
        {
            _image.color = color;
        }
    }

    public void Show(Vector2 startPos, Vector2 endPos, float duration, Ease easeType = Ease.OutBack, Action onComplete = null)
    {
        _rectTransform.anchoredPosition = startPos;
        _rectTransform
            .DOAnchorPos(endPos, duration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    public IEnumerator ShowCoroutine(Vector2 startPos, Vector2 endPos, float duration, Ease easeType = Ease.OutBack, Action onComplete = null)
    {
        _rectTransform.anchoredPosition = startPos;
        yield return _rectTransform
            .DOAnchorPos(endPos, duration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    public void Hide(Vector2 endPos, float duration, Ease easeType = Ease.OutBack, Action onComplete = null)
    {
        _rectTransform
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
        yield return _rectTransform
            .DOAnchorPos(endPos, duration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }
}