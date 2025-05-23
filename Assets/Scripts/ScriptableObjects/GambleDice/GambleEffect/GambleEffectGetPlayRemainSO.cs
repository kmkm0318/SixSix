using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectGetPlayRemainSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectGetPlayRemainSO")]
public class GambleEffectGetPlayRemainSO : GambleEffectSO
{
    [SerializeField] private int value = 1;

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        PlayManager.Instance.PlayRemain += value;
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(GambleDiceSO availityDiceSO)
    {
        return string.Format(description, value);
    }
}