using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public enum AnimationType
{
    None,
    Shake,
    Text
}

public class AnimationManager : Singleton<AnimationManager>
{
    public IEnumerator PlayAnimation<T>(T target, AnimationType animationType) where T : MonoBehaviour
    {
        switch (animationType)
        {
            default:
            case AnimationType.None:
                yield return StartCoroutine(PlayNoneAnimation());
                break;
            case AnimationType.Shake:
                if (target.TryGetComponent<Transform>(out var transform))
                {
                    yield return StartCoroutine(PlayShakeAnimation(transform));
                }
                else
                {
                    yield return StartCoroutine(PlayNoneAnimation());
                }
                break;
            case AnimationType.Text:
                if (target.TryGetComponent<TMP_Text>(out var text))
                {
                    yield return StartCoroutine(PlayTextAnimation(text));
                }
                else
                {
                    yield return StartCoroutine(PlayNoneAnimation());
                }
                break;
        }
    }

    private IEnumerator PlayNoneAnimation()
    {
        yield return null;
    }

    private IEnumerator PlayShakeAnimation(Transform transform)
    {
        var currentTween =
        transform
        .DOShakeRotation(0.5f, new Vector3(0, 0, 30), 25, 90, true, ShakeRandomnessMode.Harmonic);
        yield return currentTween.WaitForCompletion();
    }

    private IEnumerator PlayTextAnimation(TMP_Text text)
    {
        string originalText = text.text;
        text.text = string.Empty;

        var currentTween = text.DOText(originalText, 0.5f);

        yield return currentTween.WaitForCompletion();
    }
}
