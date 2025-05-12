using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectMoneySO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectMoneySO")]
public class AvailityEffectMoneySO : AvailityEffectSO
{
    [SerializeField] private int moneyAmount;

    public override void TriggerEffect(AvailityDiceContext context)
    {
        int money = GetCalculatedEffectValue(moneyAmount, context.availtiyDice.DiceValue);

        MoneyManager.Instance.Money += money;
        TriggerAnimationManager.Instance.PlayTriggerAnimation(context.availtiyDice.transform, Vector3.down, money);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(AvailityDiceSO availityDiceSO)
    {
        string res = $"Get Money({moneyAmount})";

        res += GetCalculateDescription(availityDiceSO.MaxDiceValue);

        return res;
    }
}