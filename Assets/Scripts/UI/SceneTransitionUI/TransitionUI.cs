using System;
using DG.Tweening;
using UnityEngine;

public class TransitionUI : MonoBehaviour
{
    private const string FADE = "_Fade";

    [SerializeField] private Material material;

    private Tween fadeTween;

    public void FadeIn(float duration, Action onComplete = null)
    {
        fadeTween?.Kill(true);

        fadeTween = material.DOFloat(0f, FADE, duration)
        .From(1f)
        .OnComplete(() => onComplete?.Invoke());
    }

    public void FadeOut(float duration, Action onComplete = null)
    {
        fadeTween?.Kill(true);

        fadeTween = material.DOFloat(1f, FADE, duration)
        .From(0f)
        .OnComplete(() => onComplete?.Invoke());
    }
}