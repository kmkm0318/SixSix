using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum UIAnimationType
{
    None,
    Shake,
}

public class UIAnimationManager : Singleton<UIAnimationManager>
{
    public IEnumerator PlayAnimation<T>(T target, UIAnimationType animationType) where T : MonoBehaviour
    {
        if (target.TryGetComponent<RectTransform>(out var rectTransform))
        {
            switch (animationType)
            {
                default:
                case UIAnimationType.None:
                    yield return StartCoroutine(PlayNoneAnimation(rectTransform));
                    break;
                case UIAnimationType.Shake:
                    yield return StartCoroutine(PlayShakeAnimation(rectTransform));
                    break;
            }
        }
        else
        {
            Debug.LogWarning($"Target {target.name} does not have a RectTransform component. Animation will not be played.");
            yield return null;
        }
    }

    private IEnumerator PlayNoneAnimation(RectTransform _)
    {
        yield return null;
    }

    private IEnumerator PlayShakeAnimation(RectTransform rectTransform)
    {
        var currentTween = rectTransform.DOShakeRotation(0.5f, new Vector3(0, 0, 30), 25, 90, true, ShakeRandomnessMode.Harmonic);
        yield return currentTween.WaitForCompletion();
    }
}
