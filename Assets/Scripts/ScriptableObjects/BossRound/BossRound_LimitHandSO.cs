using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitHandSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitHandSO")]
public class BossRound_LimitHandSO : BossRoundSO
{
    [SerializeField] private HandSO targetHandSO;

    public override void OnEnter()
    {
        HandManager.Instance.UsableHands = new List<Hand> { targetHandSO.hand };
    }

    public override void OnExit()
    {
        HandManager.Instance.UsableHands = null;
    }

    public override string GetBossDescription()
    {
        if (bossDescription == null)
        {
            Debug.LogError("Boss description is not set for " + name);
            return "Error: No description available.";
        }

        bossDescription.Arguments = new object[] { targetHandSO.HandName };
        bossDescription.RefreshString();
        return bossDescription.GetLocalizedString();
    }
}