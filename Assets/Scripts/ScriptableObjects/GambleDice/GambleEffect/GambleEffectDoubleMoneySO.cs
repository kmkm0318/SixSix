using UnityEngine;

[CreateAssetMenu(fileName = "GambleEffectGetMoneySO", menuName = "Scriptable Objects/GambleEffects/GambleEffectGetMoneySO")]
public class GambleEffectGetMoneySO : GambleEffectSO
{
    [SerializeField] private int amount = 20;

    public override void TriggerEffect(GambleDice gambleDice)
    {
        TriggerAnimationManager.Instance.PlayTriggerMoneyAnimation(gambleDice.transform, Vector3.down, amount);
        MoneyManager.Instance.AddMoney(amount);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(GambleDiceSO gambleDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for " + name);
            return string.Empty;
        }
        effectDescription.Arguments = new object[] { amount };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }
}