using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectRerollCostSO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectRerollCostSO")]
public class AvailityEffectRerollCostSO : AvailityEffectSO
{
    public override void TriggerEffect(AvailityDiceContext context)
    {
        ShopManager.Instance.RerollCost -= context.availtiyDice.DiceValue;
        TriggerAnimationManager.Instance.PlayTriggerAnimation(context.availtiyDice.transform);
    }

    public override string GetEffectDescription(AvailityDiceSO availityDiceSO)
    {
        string res = $"Reroll Cost -1";

        res += GetCalculateDescription(availityDiceSO.MaxDiceValue);

        return res;
    }
}