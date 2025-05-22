using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectMoneySO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectMoneySO")]
public class AvailityEffectMoneySO : AvailityEffectSO
{
    [SerializeField] private int moneyAmount;

    public override void TriggerEffect(AvailityDiceContext context)
    {
        int money = DiceEffectCalculator.GetCalculatedEffectValue(moneyAmount, context.availtiyDice.DiceValue, calculateType);

        MoneyManager.Instance.Money += money;
        TriggerAnimationManager.Instance.PlayTriggerAnimation(context.availtiyDice.transform, Vector3.down, money);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(AvailityDiceSO availityDiceSO)
    {
        string res = $"Get Money(<color={{3}}>{moneyAmount}</color>)";

        res += DiceEffectCalculator.GetCalculateDescription(availityDiceSO.MaxDiceValue, calculateType);

        return res;
    }
}