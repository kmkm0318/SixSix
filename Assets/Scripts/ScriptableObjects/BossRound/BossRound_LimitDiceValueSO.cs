using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitDiceValueSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitDiceValueSO")]
public class BossRound_LimitDiceValueSO : BossRoundSO
{
    [SerializeField] private List<int> targetDiceValues;

    public override void OnEnter()
    {
        DiceManager.Instance.UsableDiceValues = targetDiceValues;
    }

    public override void OnExit()
    {
        DiceManager.Instance.UsableDiceValues = null;
    }

    public override string GetBossDescription()
    {
        if (bossDescription == null)
        {
            Debug.LogError("Boss description is not set for " + name);
            return "Error: No description available.";
        }

        bossDescription.Arguments = new object[] { string.Join(", ", targetDiceValues) };
        bossDescription.RefreshString();
        return bossDescription.GetLocalizedString();
    }
}