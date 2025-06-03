using UnityEngine;

public class TriggerAnimationManager : Singleton<TriggerAnimationManager>
{
    public void PlayTriggerAnimation(Transform targetTransform)
    {
        AnimationFunction.AddShakeAnimation(targetTransform, false, true);
    }

    public void PlayTriggerScoreAnimation(Transform targetTransform, Vector3 offset, ScorePair scorePair)
    {
        if (scorePair.baseScore == 0 && (scorePair.multiplier == 0 || scorePair.multiplier == 1)) return;

        PlayTriggerAnimation(targetTransform);
        SequenceManager.Instance.AddCoroutine(TriggerContextUI.Instance.ShowScoreContext(targetTransform, offset, scorePair), true);
    }

    public void PlayTriggerMoneyAnimation(Transform targetTransform, Vector3 offset, int money)
    {
        if (money == 0) return;

        PlayTriggerAnimation(targetTransform);
        SequenceManager.Instance.AddCoroutine(TriggerContextUI.Instance.ShowMoneyContext(targetTransform, offset, money), true);
    }

    public void PlayTriggerValueAnimation(Transform targetTransform, Vector3 offset, int value, string color)
    {
        if (value == 0) return;

        PlayTriggerAnimation(targetTransform);
        SequenceManager.Instance.AddCoroutine(TriggerContextUI.Instance.ShowValueContext(targetTransform, offset, value, color), true);
    }
}
