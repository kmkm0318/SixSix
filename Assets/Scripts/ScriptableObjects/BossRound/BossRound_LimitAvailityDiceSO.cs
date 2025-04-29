using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitAvailityDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitAvailityDiceSO")]
public class BossRound_LimitAvailityDiceSO : BossRoundSO
{
    private AvailityDice disabledAvailityDice;

    public override void OnEnter()
    {
        disabledAvailityDice = PlayerDiceManager.Instance.GetRandomAvailityDice();

        if (disabledAvailityDice == null) return;

        PlayerDiceManager.Instance.DisableAvailityDice(disabledAvailityDice);
    }

    public override void OnExit()
    {
        if (disabledAvailityDice == null) return;

        PlayerDiceManager.Instance.EnableAvailityDice(disabledAvailityDice);
    }
}