using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectEnhancePlayDiceSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectEnhancePlayDiceSO")]
public class GambleEffectEnhancePlayDiceSO : GambleEffectSO
{
    [SerializeField] private ScorePair value = new(0, 0);

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();

        var playDiceList = DiceManager.Instance.PlayDiceList;
        foreach (var playDice in playDiceList)
        {
            playDice.EnhanceDice(value);
        }
    }

    public override string GetEffectDescription(GambleDiceSO availityDiceSO)
    {
        return string.Format(description, value);
    }
}