using System;
using DG.Tweening;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    private const float ANIMATION_DURATION = 0.5f;

    [SerializeField] protected RectTransform _panel;
    [SerializeField] protected FadeCanvasGroup _background;
    [SerializeField] protected Vector3 _showPos = Vector3.zero;
    [SerializeField] protected Vector3 _hidePos = new(0, -1000, 0);

    protected Tween _currentTween;

    protected virtual void Show(Action onComplete = null)
    {
        gameObject.SetActive(true);
        _currentTween?.Kill(true);
        _currentTween = _panel
            .DOAnchorPos(_showPos, ANIMATION_DURATION).SetEase(Ease.InOutBack)
            .From(_hidePos)
            .OnComplete(() => onComplete?.Invoke());

        if (_background != null)
        {
            _background.FadeIn(AnimationFunction.DefaultDuration);
        }
    }

    protected virtual void Hide(Action onComplete = null)
    {
        _currentTween?.Kill(true);
        _currentTween = _panel
            .DOAnchorPos(_hidePos, ANIMATION_DURATION).SetEase(Ease.InOutBack)
            .From(_showPos)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        if (_background != null)
        {
            _background.FadeOut(AnimationFunction.DefaultDuration);
        }
    }
}