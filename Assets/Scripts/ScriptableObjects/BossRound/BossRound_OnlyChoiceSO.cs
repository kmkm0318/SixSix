using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_OnlyChoiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_OnlyChoiceSO")]
public abstract class BossRound_OnlyChoiceSO : BossRoundSO
{
    [SerializeField] private HandSO targetHandSO;
    public override void OnEnter()
    {
        ScoreManager.Instance.OnHandApplied += OnHandApplied;
    }

    private void OnHandApplied(HandSO sO)
    {
        if (sO != targetHandSO)
        {

        }
    }

    public override void OnExit()
    {
        ScoreManager.Instance.OnHandApplied -= OnHandApplied;
    }
}