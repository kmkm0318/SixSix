using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitDiceValueSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitDiceValueSO")]
public class BossRound_LimitDiceValueSO : BossRoundSO
{
    [SerializeField] private List<int> targetDiceValues;

    public override void OnEnter()
    {
        PlayerDiceManager.Instance.UsableDiceValues = targetDiceValues;
    }

    public override void OnExit()
    {
        PlayerDiceManager.Instance.UsableDiceValues = null;
    }
}