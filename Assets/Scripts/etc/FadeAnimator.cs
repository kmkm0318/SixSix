using System;
using UnityEngine;

public class FadeAnimator : MonoBehaviour
{
    private const string fadeInTriggerName = "FadeIn";
    private const string fadeOutTriggerName = "FadeOut";

    [SerializeField] private Animator animator;

    private Action onFadeInComplete;
    private Action onFadeOutComplete;

    public void FadeIn(Action onComplete = null)
    {
        onFadeInComplete = onComplete;
        animator.SetTrigger(fadeInTriggerName);
    }

    public void FadeOut(Action onComplete = null)
    {
        onFadeOutComplete = onComplete;
        animator.SetTrigger(fadeOutTriggerName);
    }

    public void OnFadeInComplete()
    {
        onFadeInComplete?.Invoke();
        onFadeInComplete = null;
    }

    public void OnFadeOutComplete()
    {
        onFadeOutComplete?.Invoke();
        onFadeOutComplete = null;
    }
}