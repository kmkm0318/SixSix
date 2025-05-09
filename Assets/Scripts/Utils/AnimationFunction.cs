using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public static class AnimationFunction
{
    public static IEnumerator PlayShakeAnimation(Transform targetTransform, bool isReset = true)
    {
        var currentTween = targetTransform
        .DOShakeRotation(DataContainer.Instance.DefaultDuration, new Vector3(0, 0, 30), 25, 90, true, ShakeRandomnessMode.Harmonic)
        .OnComplete(() =>
        {
            if (isReset)
            {
                targetTransform.localRotation = Quaternion.identity;
            }
        });
        yield return currentTween.WaitForCompletion();
    }

    public static IEnumerator PlayTextAnimation(TMP_Text text, string targetText)
    {
        var currentTween = text.DOText(targetText, DataContainer.Instance.DefaultDuration);

        yield return currentTween.WaitForCompletion();
    }
}
