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
}