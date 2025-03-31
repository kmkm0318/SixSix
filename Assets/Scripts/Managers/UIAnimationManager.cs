using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UIAnimationManager : Singleton<UIAnimationManager>
{
    public enum AnimationType
    {
        None,
        Shake,
    }
    private Tween currentTween;

    public void AddAnimation(RectTransform rectTransform, AnimationType animationType)
    {
        switch (animationType)
        {
            case AnimationType.Shake:
                SequenceManager.Instance.AddCoroutine(PlayShakeAnimation(rectTransform));
                break;
            default:
                Debug.LogWarning($"Animation type {animationType} is not implemented.");
                break;
        }
    }

    private IEnumerator PlayShakeAnimation(RectTransform rectTransform)
    {
        currentTween?.Kill();
        currentTween = rectTransform.DOShakeRotation(0.5f, new Vector3(0, 0, 30), 25, 90, true, ShakeRandomnessMode.Harmonic);
        yield return currentTween.WaitForCompletion();
        currentTween = null;
    }
}
