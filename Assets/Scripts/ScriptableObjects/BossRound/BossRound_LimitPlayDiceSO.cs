using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitPlayDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitPlayDiceSO")]
public class BossRound_LimitPlayDiceSO : BossRoundSO
{
    [SerializeField] private int limitCount = 1;
    private List<PlayDice> disabledPlayDiceList;

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

    private void EnableDices()
    {
        if (disabledPlayDiceList == null) return;

        foreach (var playDice in disabledPlayDiceList)
        {
            if (playDice == null) continue;
            DiceManager.Instance.EnablePlayDice(playDice);
        }
    }

    private void DisableDices()
    {
        disabledPlayDiceList = DiceManager.Instance.GetRandomPlayDiceList(limitCount);

        if (disabledPlayDiceList == null) return;

        foreach (var playDice in disabledPlayDiceList)
        {
            if (playDice == null) continue;
            DiceManager.Instance.DisablePlayDice(playDice);
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