using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectDoubleMoneySO", menuName = "Scriptable Objects/GambleEffects/GambleEffectDoubleMoneySO")]
public class GambleEffectDoubleMoneySO : GambleEffectSO
{
    [SerializeField] private int maximum = 20;

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        int addValue = Mathf.Clamp(MoneyManager.Instance.Money, 0, maximum);
        MoneyManager.Instance.AddMoney(addValue);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(GambleDiceSO gambleDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for " + name);
            return string.Empty;
        }
        effectDescription.Arguments = new object[] { maximum };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }
}