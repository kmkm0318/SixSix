using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectDoubleMoneySO", menuName = "Scriptable Objects/GambleEffects/GambleEffectDoubleMoneySO")]
public class GambleEffectDoubleMoneySO : GambleEffectSO
{
    [SerializeField] private int value = 20;

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(gambleDice.transform);
        int addValue = Mathf.Clamp(MoneyManager.Instance.Money, 0, value);
        MoneyManager.Instance.AddMoney(addValue);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(GambleDiceSO availityDiceSO)
    {
        return string.Format(description, value);
    }
}