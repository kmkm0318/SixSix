using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectScorePairSO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectScorePairSO")]
public class AvailityEffectScorePairSO : AvailityEffectSO
{
    [SerializeField] private ScorePair scorePair;

    public override void TriggerEffect(AvailityDiceContext context)
    {
        float baseScore = GetCalculatedEffectValue(scorePair.baseScore, context.availtiyDice.DiceValue);
        float multiplier = GetCalculatedEffectValue(scorePair.multiplier, context.availtiyDice.DiceValue);

        ScorePair tmp = new(baseScore, multiplier);

        ScoreManager.Instance.ApplyScorePair(tmp);
        TriggerAnimationManager.Instance.PlayTriggerAnimation(context.availtiyDice.transform, Vector3.down, tmp);
    }

    public override string GetEffectDescription(AvailityDiceSO availityDiceSO)
    {
        List<string> parts = new();

        if (scorePair.baseScore != 0)
        {
            parts.Add($"+{scorePair.baseScore}");
        }

        if (scorePair.multiplier != 0)
        {
            parts.Add($"x{scorePair.multiplier}");
        }

        string res = "Get Score(" + string.Join(", ", parts) + ")";

        res += GetCalculateDescription(availityDiceSO.MaxDiceValue);

        return res;
    }
}