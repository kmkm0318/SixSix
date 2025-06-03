using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectGetPlayRemainSO", menuName = "Scriptable Objects/GambleEffects/GambleEffectGetPlayRemainSO")]
public class GambleEffectGetPlayRemainSO : GambleEffectSO
{
    [SerializeField] private int value = 1;

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerValueAnimation(gambleDice.transform, Vector3.down, value, "blue");
        PlayManager.Instance.PlayRemain += value;
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