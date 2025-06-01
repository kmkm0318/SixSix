using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffectMoneySO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectMoneySO")]
public class AbilityEffectMoneySO : AbilityEffectSO
{
    [SerializeField] private int moneyAmount;

    public override void TriggerEffect(AbilityDiceContext context)
    {
        int money = DiceEffectCalculator.GetCalculatedEffectValue(moneyAmount, context.currentAbilityDice.DiceValue, calculateType);

        MoneyManager.Instance.Money += money;
        TriggerAnimationManager.Instance.PlayTriggerAnimation(context.currentAbilityDice.transform, Vector3.down, money);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for AbilityEffectMoneySO.");
            return string.Empty;
        }
        effectDescription.Arguments = new object[] { moneyAmount, DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType) };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }
}