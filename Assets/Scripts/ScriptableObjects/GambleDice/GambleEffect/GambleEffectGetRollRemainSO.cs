using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectGetRollRemainSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectGetRollRemainSO")]
public class GambleEffectGetRollRemainSO : GambleEffectSO
{
    public override void TriggerEffect(GambleDice gambleDice)
    {
        RollManager.Instance.RollRemain += gambleDice.DiceValue;
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(GambleDiceSO availityDiceSO)
    {
        return $"Get Roll Remain {DiceEffectCalculator.GetCalculateDescription(availityDiceSO.MaxDiceValue, EffectCalculateType.None)}";
    }
}