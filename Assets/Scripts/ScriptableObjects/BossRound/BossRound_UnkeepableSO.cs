using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_UnkeepableSO", menuName = "Scriptable Objects/BossRounds/BossRound_UnkeepableSO")]
public class BossRound_UnkeepableSO : BossRoundSO
{
    public override void OnEnter()
    {
        RollManager.Instance.OnRollStarted += OnRollStarted;
    }

    private void OnRollStarted()
    {
        PlayerDiceManager.Instance.UnkeepAllDices();
    }

    public override void OnExit()
    {
        RollManager.Instance.OnRollStarted -= OnRollStarted;
    }
}