using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_RollOnceSO", menuName = "Scriptable Objects/BossRounds/BossRound_RollOnceSO")]
public class BossRound_RollOnceSO : BossRoundSO
{
    public override void OnEnter()
    {
        RollManager.Instance.SetCurrentRollMax(1, true);
    }

    public override void OnExit()
    {
        RollManager.Instance.SetCurrentRollMax(-1, true);
    }
}