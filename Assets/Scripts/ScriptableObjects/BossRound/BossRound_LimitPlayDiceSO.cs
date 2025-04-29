using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitPlayDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitPlayDiceSO")]
public class BossRound_LimitPlayDiceSO : BossRoundSO
{
    private PlayDice disabledPlayDice;

    public override void OnEnter()
    {
        disabledPlayDice = PlayerDiceManager.Instance.GetRandomPlayDice();

        if (disabledPlayDice == null) return;

        PlayerDiceManager.Instance.DisablePlayDice(disabledPlayDice);
    }

    public override void OnExit()
    {
        if (disabledPlayDice == null) return;

        PlayerDiceManager.Instance.EnablePlayDice(disabledPlayDice);
    }
}