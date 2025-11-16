using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "AbilityEffectMoneySO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectMoneySO")]
public class AbilityEffectMoneySO : AbilityEffectSO
{
    [SerializeField] private int moneyAmount;

    public override void TriggerEffect(AbilityDiceContext context)
    {
        int money = DiceEffectCalculator.GetCalculatedEffectValue(moneyAmount, context.currentAbilityDice.DiceValue, calculateType);

        MoneyManager.Instance.Money += money;
        TriggerAnimationManager.Instance.PlayTriggerMoneyAnimation(context.currentAbilityDice.transform, Vector3.down, money);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO, int effectValue = 0)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for AbilityEffectMoneySO.");
            return string.Empty;
        }

        var moneyValue = new LocalizedString("Formats", "MoneyValue");
        moneyValue.Arguments = new object[] { moneyAmount };
        moneyValue.RefreshString();
        var moneyValueString = moneyValue.GetLocalizedString();

        effectDescription.Arguments = new object[] { moneyValueString, DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType) };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }
}