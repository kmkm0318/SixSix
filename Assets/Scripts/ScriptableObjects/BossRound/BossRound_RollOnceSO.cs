using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_RollOnceSO", menuName = "Scriptable Objects/BossRounds/BossRound_RollOnceSO")]
public class BossRound_RollOnceSO : BossRoundSO
{
    private int previousRollMax;

    public override void OnEnter()
    {
        previousRollMax = RollManager.Instance.RollMax;

        RollManager.Instance.SetRollMax(1, true);
    }

    public override void OnExit()
    {
        RollManager.Instance.SetRollMax(previousRollMax, true);
    }
}