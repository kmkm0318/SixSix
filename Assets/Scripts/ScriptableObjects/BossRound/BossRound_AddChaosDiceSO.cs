using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_AddChaosDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_AddChaosDiceSO")]
public class BossRound_AddChaosDiceSO : BossRoundSO
{
    [SerializeField] private int chaosDiceCount = 1;

    public override void OnEnter()
    {
        PlayerDiceManager.Instance.GenerateChaosDices(chaosDiceCount);
    }

    public override void OnExit()
    {
        PlayerDiceManager.Instance.ClearChaosDices();
    }
}