using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectGetRollRemainSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectGetRollRemainSO")]
public class GambleEffectGetRollRemainSO : GambleEffectSO
{
    [SerializeField] private int value = 1;

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerValueAnimation(gambleDice.transform, Vector3.down, value, "red");
        RollManager.Instance.RollRemain += value;
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