using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectScorePairSO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectScorePairSO")]
public class AvailityEffectScorePairSO : AvailityEffectSO
{
    [SerializeField] private ScorePair scorePair;

    public override void TriggerEffect(AvailityDiceContext context)
    {
        float baseScore = GetCalculatedEffectValue(scorePair.baseScore, context.availtiyDice.DiceValue);
        float multiplier = GetCalculatedEffectValue(scorePair.multiplier, context.availtiyDice.DiceValue);

        ScorePair resultScorePair = new(baseScore, multiplier);

        TriggerManager.Instance.ApplyTriggerEffect(context.availtiyDice.transform, Vector3.down, resultScorePair);
    }

    public override string GetEffectDescription(AvailityDiceSO availityDiceSO)
    {
        return $"Get {scorePair}" + GetCalculateDescription(availityDiceSO.MaxDiceValue);
    }
}