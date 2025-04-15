using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public IEnumerator PlayShakeAnimation(Transform targetTransform)
    {
        yield return StartCoroutine(PlayShakeAnimationCoroutine(targetTransform));
    }

    private IEnumerator PlayShakeAnimationCoroutine(Transform targetTransform)
    {
        var currentTween = targetTransform.DOShakeRotation(0.5f, new Vector3(0, 0, 30), 25, 90, true, ShakeRandomnessMode.Harmonic);
        yield return currentTween.WaitForCompletion();
    }

    public IEnumerator PlayTextAnimation(TMP_Text text, string targetText)
    {
        yield return StartCoroutine(PlayTextAnimationCoroutine(text, targetText));
    }

    private IEnumerator PlayTextAnimationCoroutine(TMP_Text text, string targetText)
    {
        text.text = string.Empty;

        var currentTween = text.DOText(targetText, 0.5f);

        yield return currentTween.WaitForCompletion();
    }
}
