using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectScorePairSO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectScorePairSO")]
public class AvailityEffectScorePairSO : AvailityEffectSO
{
    [SerializeField] private ScorePair scorePair;

    public override void TriggerEffect(AvailityDiceContext context)
    {
        ScorePair resultScorePair = DiceEffectCalculator.GetCalculatedEffectValue(scorePair, context.availtiyDice.DiceValue, calculateType);

        TriggerManager.Instance.ApplyTriggerEffect(context.availtiyDice.transform, Vector3.down, resultScorePair);
    }

    public override string GetEffectDescription(AvailityDiceSO availityDiceSO)
    {
        return $"Get {scorePair}" + DiceEffectCalculator.GetCalculateDescription(availityDiceSO.MaxDiceValue, calculateType);
    }
}