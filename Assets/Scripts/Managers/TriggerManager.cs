using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : Singleton<TriggerManager>
{
    private void Start()
    {
        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Round, OnRoundStarted);
        GameManager.Instance.RegisterEvent(GameState.RoundClear, OnRoundClearStarted);
        GameManager.Instance.RegisterEvent(GameState.Shop, OnShopStarted, OnShopEnded);
        GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted, OnPlayEnded);
        GameManager.Instance.RegisterEvent(GameState.Roll, OnRollStarted, OnRollCompleted);
    }

    private void OnRoundStarted()
    {
        TriggerAvailityDice(EffectTriggerType.RoundStarted);
    }

    private void OnRoundClearStarted()
    {
        TriggerAvailityDice(EffectTriggerType.RoundCleared);
    }

    private void OnShopStarted()
    {
        TriggerAvailityDice(EffectTriggerType.ShopStarted);
    }

    private void OnShopEnded()
    {
        TriggerAvailityDice(EffectTriggerType.ShopEnded);
    }

    private void OnPlayStarted()
    {
        TriggerAvailityDice(EffectTriggerType.PlayStarted);
    }

    private void OnPlayEnded()
    {
        TriggerAvailityDice(EffectTriggerType.PlayEnded);
    }

    private void OnRollStarted()
    {
        TriggerAvailityDice(EffectTriggerType.RollStarted);
    }

    private void OnRollCompleted()
    {
        TriggerAvailityDice(EffectTriggerType.RollEnded);
    }
    #endregion

    #region TriggerDice
    public void TriggerDices()
    {
        TriggerGambleDices();
        TriggerPlayDices();
        TriggerChaosDices();
        TriggerAvailityDice(HandManager.Instance.LastSelectedHandSO);
    }

    private void TriggerPlayDices()
    {
        var playDiceList = DiceManager.Instance.GetOrderedPlayDiceList();
        var UsableDiceValues = DiceManager.Instance.UsableDiceValues;

        foreach (var playDice in playDiceList)
        {
            if (UsableDiceValues != null && !UsableDiceValues.Contains(playDice.DiceValue)) continue;

            playDice.ApplyScorePairs();
            TriggerAvailityDice(playDice);
        }
    }

    private void TriggerAvailityDice(EffectTriggerType triggerType, AvailityDiceContext context)
    {
        List<AvailityDice> triggeredAvailityDiceList = DiceManager.Instance.AvailityDiceList.FindAll(dice => dice.IsTriggered(triggerType, context));

        foreach (var availityDice in triggeredAvailityDiceList)
        {
            availityDice.TriggerEffect();
        }
    }

    private void TriggerAvailityDice(EffectTriggerType triggerType)
    {
        TriggerAvailityDice(triggerType, new());
    }

    private void TriggerAvailityDice(PlayDice playDice)
    {
        TriggerAvailityDice(EffectTriggerType.PlayDiceApplied, new(playDice: playDice));
    }

    private void TriggerAvailityDice(HandSO handSO)
    {
        TriggerAvailityDice(EffectTriggerType.HandApplied, new(handSO: handSO));
    }

    private void TriggerChaosDices()
    {
        foreach (var chaosDice in DiceManager.Instance.ChaosDiceList)
        {
            chaosDice.ApplyScorePairs();
        }
    }

    private void TriggerGambleDices()
    {
        foreach (var gambleDice in DiceManager.Instance.GambleDiceList)
        {
            gambleDice.TriggerEffect();
        }
    }
    #endregion

    #region ApplyTriggerEffect
    public void ApplyTriggerEffect(Transform targetTransform, Vector3 offset, ScorePair scorePair)
    {
        bool isBaseScoreZero = scorePair.baseScore == 0;
        bool isMultiplierZeroOrOne = scorePair.multiplier == 0 || scorePair.multiplier == 1;

        if (isBaseScoreZero && isMultiplierZeroOrOne) return;

        if (!isBaseScoreZero && !isMultiplierZeroOrOne)
        {
            ApplyTriggerEffect(targetTransform, offset, new ScorePair(scorePair.baseScore, 1));
            ApplyTriggerEffect(targetTransform, offset, new ScorePair(0, scorePair.multiplier));
            return;
        }

        ScoreManager.Instance.ApplyScorePair(scorePair);
        TriggerAnimationManager.Instance.PlayTriggerAnimation(targetTransform, offset, scorePair);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public void ApplyTriggerEffect(Transform targetTransform, Vector3 offset, int money)
    {
        if (money == 0) return;

        MoneyManager.Instance.Money += money;
        TriggerAnimationManager.Instance.PlayTriggerAnimation(targetTransform, offset, money);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }
    #endregion
}
