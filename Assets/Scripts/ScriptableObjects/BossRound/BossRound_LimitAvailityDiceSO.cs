using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitAvailityDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitAvailityDiceSO")]
public class BossRound_LimitAvailityDiceSO : BossRoundSO
{
    private AvailityDice disabledAvailityDice;

    public override void OnEnter()
    {
        disabledAvailityDice = PlayerDiceManager.Instance.GetRandomAvailityDice();

        if (disabledAvailityDice == null) return;

        PlayerDiceManager.Instance.RemoveAvailityDice(disabledAvailityDice, false);
    }

    public override void OnExit()
    {
        if (disabledAvailityDice == null) return;

        disabledAvailityDice.gameObject.SetActive(true);

        PlayerDiceManager.Instance.RespawnAvailityDice(disabledAvailityDice);
    }
}