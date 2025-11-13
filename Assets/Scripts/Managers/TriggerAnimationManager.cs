using UnityEngine;

public class TriggerAnimationManager : Singleton<TriggerAnimationManager>
{
    public void PlayTriggerAnimation(Transform targetTransform)
    {
        AnimationFunction.AddShakeAnimation(targetTransform, false, true);
        SequenceManager.Instance.AddCoroutine(() => AudioManager.Instance.PlaySFX(SFXType.DiceTrigger), true);
    }

    public void PlayTriggerScoreAnimation(Transform targetTransform, Vector3 offset, ScorePair scorePair)
    {
        if (scorePair.baseScore == 0 && (scorePair.multiplier == 0 || scorePair.multiplier == 1)) return;

        PlayTriggerAnimation(targetTransform);
        TriggerContextUIEvents.TriggerOnShowScoreContext(targetTransform, offset, scorePair);
    }

    public void PlayTriggerMoneyAnimation(Transform targetTransform, Vector3 offset, int money)
    {
        if (money == 0) return;

        PlayTriggerAnimation(targetTransform);
        TriggerContextUIEvents.TriggerOnShowMoneyContext(targetTransform, offset, money);
    }

    public void PlayTriggerValueAnimation(Transform targetTransform, Vector3 offset, int value, string color)
    {
        if (value == 0) return;

        PlayTriggerAnimation(targetTransform);
        TriggerContextUIEvents.TriggerOnShowValueContext(targetTransform, offset, value, color);
    }

    public void PlayRetriggerAnimation(Transform targetTransform, Vector3 offset, string color)
    {
        PlayTriggerAnimation(targetTransform);
        TriggerContextUIEvents.TriggerOnShowRetriggerContext(targetTransform, offset, color);
    }
}
