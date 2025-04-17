using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public IEnumerator PlayShakeAnimation(Transform targetTransform, bool isReset = false)
    {
        var currentTween = targetTransform
        .DOShakeRotation(0.5f, new Vector3(0, 0, 30), 25, 90, true, ShakeRandomnessMode.Harmonic)
        .OnComplete(() =>
        {
            if (isReset)
            {
                targetTransform.localRotation = Quaternion.identity;
            }
        });
        yield return currentTween.WaitForCompletion();
    }

    public IEnumerator PlayTextAnimation(TMP_Text text, string targetText)
    {
        var currentTween = text.DOText(targetText, 0.5f);

        yield return currentTween.WaitForCompletion();
    }
}
