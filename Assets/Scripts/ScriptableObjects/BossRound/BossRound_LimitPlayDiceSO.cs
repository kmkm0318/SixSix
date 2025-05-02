using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitPlayDiceSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitPlayDiceSO")]
public class BossRound_LimitPlayDiceSO : BossRoundSO
{
    [SerializeField] private int limitCount = 1;
    private List<PlayDice> disabledPlayDiceList;

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

    private void EnableDices()
    {
        if (disabledPlayDiceList == null) return;

        foreach (var playDice in disabledPlayDiceList)
        {
            if (playDice == null) continue;
            PlayerDiceManager.Instance.EnablePlayDice(playDice);
        }
    }

    private void DisableDices()
    {
        disabledPlayDiceList = PlayerDiceManager.Instance.GetRandomPlayDiceList(limitCount);

        if (disabledPlayDiceList == null) return;

        foreach (var playDice in disabledPlayDiceList)
        {
            if (playDice == null) continue;
            PlayerDiceManager.Instance.DisablePlayDice(playDice);
        }
    }
}