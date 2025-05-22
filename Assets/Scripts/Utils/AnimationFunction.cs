using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public static class AnimationFunction
{
    public static float DefaultDuration = 0.5f;

    public static IEnumerator ShakeAnimation(Transform targetTransform, bool isReset = true)
    {
        targetTransform.DOKill(true);
        var currentTween = targetTransform
        .DOShakeRotation(DefaultDuration, new Vector3(0, 0, 30), 25, 90, true, ShakeRandomnessMode.Harmonic)
        .OnComplete(() =>
        {
            if (isReset)
            {
                targetTransform.localRotation = Quaternion.identity;
            }
        });
        yield return currentTween.WaitForCompletion();
    }

    public static void AddShakeAnimation(Transform targetTransform, bool isReset = true, bool isParallel = false)
    {
        if (SequenceManager.Instance != null)
        {
            SequenceManager.Instance.AddCoroutine(ShakeAnimation(targetTransform, isReset), isParallel);
        }
    }

    public static IEnumerator TextShowAnimation(AnimatedText text, string targetText)
    {
        yield return text.ShowTextCoroutine(targetText);
    }

    public static void AddTextShowAnimation(AnimatedText text, string targetText)
    {
        if (SequenceManager.Instance != null)
        {
            SequenceManager.Instance.AddCoroutine(TextShowAnimation(text, targetText));
        }
    }

    public static void AddUpdateTextAndPlayAnimation(AnimatedText text, string targetText, bool isParallel = false)
    {
        if (SequenceManager.Instance != null)
        {
            SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(text, targetText), isParallel);
        }
    }

    public static void AddUpdateTextAndPlayAnimation(AnimatedText text, int value, bool isParallel = false)
    {
        AddUpdateTextAndPlayAnimation(text, value.ToString(), isParallel);
    }

    public static void AddUpdateTextAndPlayAnimation(AnimatedText text, double value, bool isParallel = false)
    {
        AddUpdateTextAndPlayAnimation(text, UtilityFunctions.FormatNumber(value), isParallel);
    }

    public static IEnumerator UpdateTextAndPlayAnimation(AnimatedText text, string targetText)
    {
        text.SetText(targetText);
        yield return ShakeAnimation(text.transform);
    }
}
