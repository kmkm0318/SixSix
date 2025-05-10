using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectDiscountRerollCostSO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectDiscountRerollCostSO")]
public class AvailityEffectDiscountRerollCostSO : AvailityEffectSO
{
    public override void TriggerEffect(AvailityDiceContext context)
    {
        ShopManager.Instance.RerollCost -= context.availtiyDice.DiceValue;
        TriggerAnimationManager.Instance.PlayTriggerAnimation(context.availtiyDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public override string GetEffectDescription(AvailityDiceSO availityDiceSO)
    {
        string res = $"Reroll Cost -1";

        res += GetCalculateDescription(availityDiceSO.MaxDiceValue);

        return res;
    }
}