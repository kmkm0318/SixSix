using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitPlayDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitPlayDiceSO")]
public abstract class BossRound_LimitPlayDiceSO : BossRoundSO
{
    private PlayDice disabledPlayDice;

    public override void OnEnter()
    {
        disabledPlayDice = PlayerDiceManager.Instance.GetRandomPlayDice();
        PlayerDiceManager.Instance.RemovePlayDice(disabledPlayDice);
    }

    public override void OnExit()
    {
        disabledPlayDice.gameObject.SetActive(true);
        PlayerDiceManager.Instance.RespawnPlayDice(disabledPlayDice);
    }
}