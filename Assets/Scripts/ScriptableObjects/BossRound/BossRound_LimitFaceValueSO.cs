using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitFaceValueSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitFaceValueSO")]
public class BossRound_LimitFaceValueSO : BossRoundSO
{
    [SerializeField] private List<int> targetFaceValues;

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }
}