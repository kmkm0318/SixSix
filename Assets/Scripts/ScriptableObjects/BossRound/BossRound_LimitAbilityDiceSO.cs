using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitAbilityDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitAbilityDiceSO")]
public class BossRound_LimitAbilityDiceSO : BossRoundSO
{
    [SerializeField] private int limitCount = 1;
    private List<AbilityDice> disabledAbilityDiceList;

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
        disabledAbilityDiceList = DiceManager.Instance.GetRandomAbilityDiceList(limitCount);

        if (disabledAbilityDiceList == null) return;

        foreach (var abilityDice in disabledAbilityDiceList)
        {
            if (abilityDice == null) continue;
            DiceManager.Instance.DisableAbilityDice(abilityDice);
        }
    }

    private void EnableDices()
    {
        if (disabledAbilityDiceList == null) return;

        foreach (var abilityDice in disabledAbilityDiceList)
        {
            if (abilityDice == null) continue;
            DiceManager.Instance.EnableAbilityDice(abilityDice);
        }
    }
}