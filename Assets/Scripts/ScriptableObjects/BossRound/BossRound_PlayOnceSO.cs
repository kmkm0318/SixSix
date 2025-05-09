using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_PlayOnceSO", menuName = "Scriptable Objects/BossRounds/BossRound_PlayOnceSO")]
public class BossRound_PlayOnceSO : BossRoundSO
{
    public override void OnEnter()
    {
        PlayManager.Instance.SetCurrentPlayMax(1, true);
    }

    public override void OnExit()
    {
        PlayManager.Instance.SetCurrentPlayMax(-1, true);
    }
}