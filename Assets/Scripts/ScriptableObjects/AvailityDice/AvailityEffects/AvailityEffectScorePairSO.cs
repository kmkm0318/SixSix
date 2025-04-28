using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailityEffectScorePairSO", menuName = "Scriptable Objects/AvailityEffects/AvailityEffectScorePairSO")]
public class AvailityEffectScorePairSO : AvailityEffectSO
{
    [SerializeField] private ScorePair scorePair;

    public override void ApplyEffect(AvailityDiceContext context)
    {
        float baseScore = GetCalculatedEffectValue(scorePair.baseScore, context.availtiyDice.FaceValue);
        float multiplier = GetCalculatedEffectValue(scorePair.multiplier, context.availtiyDice.FaceValue);

        if (baseScore != 0)
        {
            ScoreManager.Instance.ApplyDiceScorePairEffectAndPlayAnimation(context.availtiyDice, new(baseScore, 0), true);
        }

        if (multiplier != 0)
        {
            ScoreManager.Instance.ApplyDiceScorePairEffectAndPlayAnimation(context.availtiyDice, new(0, multiplier), true);
        }
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

        res += GetCalculateDescription(availityDiceSO.maxFaceValue);

        return res;
    }
}