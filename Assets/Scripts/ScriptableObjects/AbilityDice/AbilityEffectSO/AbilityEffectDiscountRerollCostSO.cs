using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffectDiscountRerollCostSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectDiscountRerollCostSO")]
public class AbilityEffectDiscountRerollCostSO : AbilityEffectSO
{
    public override void TriggerEffect(AbilityDiceContext context)
    {
        ShopManager.Instance.RerollCost -= context.abilityDice.DiceValue;
        TriggerAnimationManager.Instance.PlayTriggerAnimation(context.abilityDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for AbilityEffectDiscountRerollCostSO.");
            return string.Empty;
        }

        effectDescription.Arguments = new object[] { DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType) };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }
}