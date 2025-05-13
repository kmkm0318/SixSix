using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectEnhancePlayDiceSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectEnhancePlayDiceSO")]
public class GambleEffectEnhancePlayDiceSO : GambleEffectSO
{
    public override void TriggerEffect(GambleDice gambleDice)
    {
        var enhanceLevel = DiceEffectCalculator.GetCalculatedEffectValue(gambleDice.DiceValue, gambleDice.DiceValue, EffectCalculateType.None);

        var playDiceList = DiceManager.Instance.PlayDiceList;
        foreach (var playDice in playDiceList)
        {
            playDice.EnhanceDice(enhanceLevel);
        }
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(GambleDiceSO availityDiceSO)
    {
        return $"Enhance Every Play Dice Lv.{DiceEffectCalculator.GetCalculateDescription(availityDiceSO.MaxDiceValue, EffectCalculateType.None)}";
    }
}