using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectEnhanceHandSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectEnhanceHandSO")]
public class GambleEffectEnhanceHandSO : GambleEffectSO
{
    [SerializeField] private int value = 1;

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();

        var hands = HandManager.Instance.CompletedHands;
        foreach (var hand in hands)
        {
            HandManager.Instance.EnhanceHand(hand, value);
        }
    }

    public override string GetEffectDescription(GambleDiceSO availityDiceSO)
    {
        return string.Format(description, value);
    }
}