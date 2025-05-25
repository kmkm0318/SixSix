using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectEnhancePlayDiceSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectEnhancePlayDiceSO")]
public class GambleEffectEnhancePlayDiceSO : GambleEffectSO
{
    [SerializeField] private ScorePair enhanceValue = new(0, 0);

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();

        var playDiceList = DiceManager.Instance.PlayDiceList;
        foreach (var playDice in playDiceList)
        {
            playDice.EnhanceDice(enhanceValue);
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