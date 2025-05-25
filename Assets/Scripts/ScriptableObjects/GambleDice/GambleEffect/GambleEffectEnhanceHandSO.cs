using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectEnhanceHandSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectEnhanceHandSO")]
public class GambleEffectEnhanceHandSO : GambleEffectSO
{
    [SerializeField] private int enhanceValue = 1;

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();

        var hands = HandManager.Instance.CompletedHands;
        foreach (var hand in hands)
        {
            HandManager.Instance.EnhanceHand(hand, enhanceValue);
        }
    }

    public override string GetEffectDescription(GambleDiceSO gambleDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for " + name);
            return string.Empty;
        }
        effectDescription.Arguments = new object[] { enhanceValue };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }
}