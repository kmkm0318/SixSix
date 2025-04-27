using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitAvailityDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitAvailityDiceSO")]
public abstract class BossRound_LimitAvailityDiceSO : BossRoundSO
{
    private AvailityDice disabledAvailityDice;

    public override void OnEnter()
    {
        disabledAvailityDice = PlayerDiceManager.Instance.GetRandomAvailityDice();
        PlayerDiceManager.Instance.RemoveAvailityDice(disabledAvailityDice);
    }

    public override void OnExit()
    {
        disabledAvailityDice.gameObject.SetActive(true);
        PlayerDiceManager.Instance.RespawnAvailityDice(disabledAvailityDice);
    }
}