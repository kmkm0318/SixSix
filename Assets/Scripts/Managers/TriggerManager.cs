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
        GameManager.Instance.RegisterEvent(GameState.Roll, OnRollStarted);
    }

    private void OnRoundStarted()
    {
        TriggerAbilityDice(EffectTriggerType.RoundStarted);
    }

    private void OnRoundClearStarted()
    {
        TriggerAbilityDice(EffectTriggerType.RoundCleared);
    }

    private void OnShopStarted()
    {
        TriggerAbilityDice(EffectTriggerType.ShopStarted);
    }

    private void OnShopEnded()
    {
        TriggerAbilityDice(EffectTriggerType.ShopEnded);
    }

    private void OnPlayStarted()
    {
        TriggerAbilityDice(EffectTriggerType.PlayStarted);
    }

    private void OnPlayEnded()
    {
        TriggerAbilityDice(EffectTriggerType.PlayEnded);
    }

    private void OnRollStarted()
    {
        TriggerAbilityDice(EffectTriggerType.RollStarted);
    }

    public void TriggerOnRollCompleted()
    {
        TriggerAbilityDice(EffectTriggerType.RollEnded);
        if (GameManager.Instance.CurrentGameState == GameState.Round)
        {
            Instance.TriggerGambleDices();
            SequenceManager.Instance.AddCoroutine(DiceManager.Instance.ClearGambleDices);
        }
    }
    #endregion

    #region TriggerDice
    public void TriggerDices()
    {
        TriggerPlayDices();
        TriggerChaosDices();
        TriggerAbilityDice(HandManager.Instance.LastSelectedHandSO);
    }

    private void TriggerPlayDices()
    {
        var playDiceList = DiceManager.Instance.GetOrderedPlayDiceList();
        var UsableDiceValues = DiceManager.Instance.UsableDiceValues;

        foreach (var playDice in playDiceList)
        {
            if (UsableDiceValues != null && !UsableDiceValues.Contains(playDice.DiceValue)) continue;

            playDice.ApplyScorePairs();
            TriggerAbilityDice(playDice);
        }
    }

    private void TriggerAbilityDice(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        List<AbilityDice> triggeredAbilityDiceList = DiceManager.Instance.AbilityDiceList.FindAll(dice => dice.IsTriggered(triggerType, context));

        foreach (var abilityDice in triggeredAbilityDiceList)
        {
            abilityDice.TriggerEffect();
        }
    }

    private void TriggerAbilityDice(EffectTriggerType triggerType)
    {
        TriggerAbilityDice(triggerType, new());
    }

    private void TriggerAbilityDice(PlayDice playDice)
    {
        TriggerAbilityDice(EffectTriggerType.PlayDiceApplied, new(playDice: playDice));
    }

    private void TriggerAbilityDice(HandSO handSO)
    {
        TriggerAbilityDice(EffectTriggerType.HandApplied, new(handSO: handSO));
    }

    private void TriggerChaosDices()
    {
        foreach (var chaosDice in DiceManager.Instance.ChaosDiceList)
        {
            chaosDice.ApplyScorePairs();
        }
    }

    public void TriggerGambleDices()
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
