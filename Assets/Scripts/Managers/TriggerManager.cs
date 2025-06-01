using System;
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
        TriggerAbilityDices(EffectTriggerType.RoundStarted);
    }

    private void OnRoundClearStarted()
    {
        TriggerAbilityDices(EffectTriggerType.RoundCleared);
    }

    private void OnShopStarted()
    {
        TriggerAbilityDices(EffectTriggerType.ShopStarted);
    }

    private void OnShopEnded()
    {
        TriggerAbilityDices(EffectTriggerType.ShopEnded);
    }

    private void OnPlayStarted()
    {
        TriggerAbilityDices(EffectTriggerType.PlayStarted);
    }

    private void OnPlayEnded()
    {
        TriggerAbilityDices(EffectTriggerType.PlayEnded);
    }

    private void OnRollStarted()
    {
        TriggerAbilityDices(EffectTriggerType.RollStarted);
    }

    public void TriggerOnRollCompleted()
    {
        TriggerAbilityDices(EffectTriggerType.RollEnded);
        if (GameManager.Instance.CurrentGameState == GameState.Round)
        {
            TriggerGambleDices();
            SequenceManager.Instance.AddCoroutine(DiceManager.Instance.ClearGambleDices);
        }
    }
    #endregion

    #region TriggerDice
    public void TriggerDices()
    {
        TriggerPlayDices();
        TriggerChaosDices();
        TriggerAbilityDices(HandManager.Instance.LastSelectedHandSO);
    }

    private void TriggerPlayDices()
    {
        foreach (var playDice in DiceManager.Instance.GetOrderedPlayDiceList())
        {
            TriggerPlayDice(playDice);
        }
    }

    public void TriggerPlayDice(PlayDice playDice, bool isRetriggered = false)
    {
        if (DiceManager.Instance.UsableDiceValues != null && !DiceManager.Instance.UsableDiceValues.Contains(playDice.DiceValue)) return;

        playDice.ApplyScorePairs();
        TriggerAbilityDices(playDice, isRetriggered);
    }

    private void TriggerAbilityDices(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        List<AbilityDice> triggeredAbilityDiceList = DiceManager.Instance.AbilityDiceList.FindAll(dice => dice.IsTriggered(triggerType, context));

        foreach (var abilityDice in triggeredAbilityDiceList)
        {
            TriggerAbilityDice(abilityDice, context);
        }
    }

    public void TriggerAbilityDice(AbilityDice abilityDice, AbilityDiceContext context = null, bool isRetriggered = false)
    {
        abilityDice.TriggerEffect(context);
        TriggerAbilityDices(abilityDice, isRetriggered);
    }

    private void TriggerAbilityDices(EffectTriggerType triggerType)
    {
        TriggerAbilityDices(triggerType, new());
    }

    private void TriggerAbilityDices(PlayDice playDice, bool isRetriggered = false)
    {
        TriggerAbilityDices(EffectTriggerType.PlayDiceTriggered, new(playDice: playDice, isRetriggered: isRetriggered));
    }

    private void TriggerAbilityDices(AbilityDice abilityDice, bool isRetriggered = false)
    {
        TriggerAbilityDices(EffectTriggerType.AbilityDiceTriggered, new(abilityDice: abilityDice, isRetriggered: isRetriggered));
    }

    private void TriggerAbilityDices(GambleDice gambleDice, bool isRetriggered = false)
    {
        TriggerAbilityDices(EffectTriggerType.GambleDiceTriggered, new(gambleDice: gambleDice, isRetriggered: isRetriggered));
    }

    private void TriggerAbilityDices(HandSO handSO)
    {
        TriggerAbilityDices(EffectTriggerType.HandPlayed, new(handSO: handSO));
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
        var gambleDices = DiceManager.Instance.GambleDiceList.FindAll(dice => dice.IsTriggered());
        foreach (var gambleDice in gambleDices)
        {
            TriggerGambleDice(gambleDice);
        }
    }

    public void TriggerGambleDice(GambleDice gambleDice, bool isRetriggered = false)
    {
        gambleDice.TriggerEffect();
        TriggerAbilityDices(gambleDice, isRetriggered);
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
