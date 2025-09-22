using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_DisableAbilityDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_DisableAbilityDiceSO")]
public class BossRound_DisableAbilityDiceSO : BossRoundSO
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
        disabledAbilityDiceList = DiceManager.Instance.AbilityDiceList.GetRandomElements(limitCount);

        if (ListUtils.IsNullOrEmpty(disabledAbilityDiceList)) return;

        foreach (var abilityDice in disabledAbilityDiceList)
        {
            if (abilityDice == null) continue;
            DiceManager.Instance.DisableAbilityDice(abilityDice);
        }
    }

    private void EnableDices()
    {
        if (ListUtils.IsNullOrEmpty(disabledAbilityDiceList)) return;

        foreach (var abilityDice in disabledAbilityDiceList)
        {
            if (abilityDice == null) continue;
            DiceManager.Instance.EnableAbilityDice(abilityDice);
        }
    }

    public override string GetBossDescription()
    {
        if (bossDescription == null)
        {
            Debug.LogError("Boss description is not set for " + name);
            return "Error: No description available.";
        }

        bossDescription.Arguments = new object[] { limitCount };
        bossDescription.RefreshString();
        return bossDescription.GetLocalizedString();
    }
}