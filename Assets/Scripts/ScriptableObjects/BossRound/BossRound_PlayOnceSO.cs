using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_PlayOnceSO", menuName = "Scriptable Objects/BossRounds/BossRound_PlayOnceSO")]
public abstract class BossRound_PlayOnceSO : BossRoundSO
{
    private int previousPlayMax;

    public override void OnEnter()
    {
        previousPlayMax = PlayManager.Instance.PlayMax;

        PlayManager.Instance.SetPlayMax(1, true);
    }

    public override void OnExit()
    {
        PlayManager.Instance.SetPlayMax(previousPlayMax, true);
    }
}