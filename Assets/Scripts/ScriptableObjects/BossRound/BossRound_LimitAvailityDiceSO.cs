using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitAvailityDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitAvailityDiceSO")]
public class BossRound_LimitAvailityDiceSO : BossRoundSO
{
    [SerializeField] private int limitCount = 1;
    private List<AvailityDice> disabledAvailityDiceList;

    public override void OnEnter()
    {
        PlayManager.Instance.OnPlayStarted += OnPlayStarted;
        DisableDices();
    }

    public override void OnExit()
    {
        EnableDices();
        PlayManager.Instance.OnPlayStarted -= OnPlayStarted;
    }

    private void OnPlayStarted(int obj)
    {
        SequenceManager.Instance.AddCoroutine(() =>
        {
            EnableDices();
            DisableDices();
        });
    }

    private void DisableDices()
    {
        disabledAvailityDiceList = PlayerDiceManager.Instance.GetRandomAvailityDiceList(limitCount);

        if (disabledAvailityDiceList == null) return;

        foreach (var availityDice in disabledAvailityDiceList)
        {
            if (availityDice == null) continue;
            PlayerDiceManager.Instance.DisableAvailityDice(availityDice);
        }
    }

    private void EnableDices()
    {
        if (disabledAvailityDiceList == null) return;

        foreach (var availityDice in disabledAvailityDiceList)
        {
            if (availityDice == null) continue;
            PlayerDiceManager.Instance.EnableAvailityDice(availityDice);
        }
    }
}