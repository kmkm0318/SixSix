using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitAvailityDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitAvailityDiceSO")]
public class BossRound_LimitAvailityDiceSO : BossRoundSO
{
    [SerializeField] private int limitCount = 1;
    private List<AvailityDice> disabledAvailityDiceList;

    public override void OnEnter()
    {
        GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted);
        DisableDices();
    }

    public override void OnExit()
    {
        EnableDices();
        GameManager.Instance.UnregisterEvent(GameState.Play, OnPlayStarted);
    }

    private void OnPlayStarted()
    {
        SequenceManager.Instance.AddCoroutine(() =>
        {
            EnableDices();
            DisableDices();
        });
    }

    private void DisableDices()
    {
        disabledAvailityDiceList = DiceManager.Instance.GetRandomAvailityDiceList(limitCount);

        if (disabledAvailityDiceList == null) return;

        foreach (var availityDice in disabledAvailityDiceList)
        {
            if (availityDice == null) continue;
            DiceManager.Instance.DisableAvailityDice(availityDice);
        }
    }

    private void EnableDices()
    {
        if (disabledAvailityDiceList == null) return;

        foreach (var availityDice in disabledAvailityDiceList)
        {
            if (availityDice == null) continue;
            DiceManager.Instance.EnableAvailityDice(availityDice);
        }
    }
}