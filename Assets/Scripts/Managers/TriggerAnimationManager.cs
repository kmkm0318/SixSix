using UnityEngine;

public class TriggerAnimationManager : Singleton<TriggerAnimationManager>
{
    public void PlayTriggerAnimation(Transform targetTransform)
    {
        SequenceManager.Instance.AddCoroutine(AnimationFunction.PlayShakeAnimation(targetTransform, false), true);
    }

    public void PlayTriggerAnimation(Transform targetTransform, Vector3 offset, ScorePair scorePair)
    {
        if (scorePair.baseScore == 0 && (scorePair.multiplier == 0 || scorePair.multiplier == 1)) return;

        SequenceManager.Instance.AddCoroutine(AnimationFunction.PlayShakeAnimation(targetTransform, false), true);
        SequenceManager.Instance.AddCoroutine(TriggerContextUI.Instance.ShowContext(targetTransform, offset, scorePair), true);
    }

    public void PlayTriggerAnimation(Transform targetTransform, Vector3 offset, int money)
    {
        if (money == 0) return;

        SequenceManager.Instance.AddCoroutine(AnimationFunction.PlayShakeAnimation(targetTransform, false), true);
        SequenceManager.Instance.AddCoroutine(TriggerContextUI.Instance.ShowContext(targetTransform, offset, money), true);
    }
}
