using System.Collections.Generic;
using UnityEngine;

public class RoundClearState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();
        if (RoundManager.Instance.IsBossRound)
        {
            SequenceManager.Instance.AddCoroutine(() =>
            {
                GambleDiceSaveManager.Instance.TryAddRandomBossGambleDiceIcon();
            });
        }
        
        SaveRunAbilityDiceData();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SaveRunAbilityDiceData()
    {
        int clearedRound = RoundManager.Instance.CurrentRound;
        List<int> diceIDs = new();

        var abilityDiceList = DiceManager.Instance.AbilityDiceList;

        foreach (var abilityDice in abilityDiceList)
        {
            diceIDs.Add(abilityDice.AbilityDiceSO.abilityDiceID);
        }

        DatabaseManager.Instance.SaveGameRun(diceIDs, clearedRound);
    }
}