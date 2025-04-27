using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectMoneySO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectMoneySO")]
public class AvailityEffectMoneySO : AvailityEffectSO
{
    [SerializeField] private int moneyAmount;

    public override void ApplyEffect(AvailityDiceContext context)
    {
        int money = CalculateEffectValue(moneyAmount, context.availtiyDice.FaceValue);

        ScoreManager.Instance.ApplyMoneyAndPlayDiceAnimation(context.availtiyDice, money, true);
    }

    public override string GetEffectDescription(AvailityDiceSO availityDiceSO)
    {
        string res = $"Get Money({moneyAmount})";

        res += GetCalculateDescription(availityDiceSO.maxFaceValue);

        return res;
    }
}