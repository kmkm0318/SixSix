using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectGetRollRemainSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectGetRollRemainSO")]
public class GambleEffectGetRollRemainSO : GambleEffectSO
{
    [SerializeField] private int value = 1;

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        RollManager.Instance.RollRemain += value;
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(GambleDiceSO availityDiceSO)
    {
        return string.Format(description, value);
    }
}