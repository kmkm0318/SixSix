using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_UnkeepableSO", menuName = "Scriptable Objects/BossRounds/BossRound_UnkeepableSO")]
public class BossRound_UnkeepableSO : BossRoundSO
{
    public override void OnEnter()
    {
        DiceManager.Instance.IsKeepable = false;
    }

    public override void OnExit()
    {
        DiceManager.Instance.IsKeepable = true;
    }
}