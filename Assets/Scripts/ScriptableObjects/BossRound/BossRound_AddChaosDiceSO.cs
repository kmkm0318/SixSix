using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_AddChaosDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_AddChaosDiceSO")]
public class BossRound_AddChaosDiceSO : BossRoundSO
{
    [SerializeField] private int chaosDiceCount = 1;

    public override void OnEnter()
    {
        DiceManager.Instance.StartGenerateChaosDice(chaosDiceCount);
    }

    public override void OnExit()
    {
        DiceManager.Instance.ClearChaosDices();
    }
}