using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectGetRollMaxSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectGetRollMaxSO")]
public class GambleEffectGetRollMaxSO : GambleEffectSO
{
    [SerializeField] private int value = 1;

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        RollManager.Instance.IncreaseRollMaxAndRemain(value);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(GambleDiceSO gambleDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for " + name);
            return string.Empty;
        }
        effectDescription.Arguments = new object[] { value };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }
}