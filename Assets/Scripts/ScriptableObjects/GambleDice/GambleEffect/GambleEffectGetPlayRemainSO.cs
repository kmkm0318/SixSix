using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectGetRollRemainSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectGetRollRemainSO")]
public class GambleEffectGetPlayRemainSO : GambleEffectSO
{
    public override void TriggerEffect(GambleDice gambleDice)
    {
        PlayManager.Instance.PlayRemain += gambleDice.DiceValue;
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(GambleDiceSO availityDiceSO)
    {
        return $"Get Play Remain {DiceEffectCalculator.GetCalculateDescription(availityDiceSO.MaxDiceValue, EffectCalculateType.None)}";
    }
}