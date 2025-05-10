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
        RoundManager.Instance.OnRoundStarted += OnRoundStarted;
        RoundClearManager.Instance.OnRoundClearStarted += OnRoundClearStarted;
        ShopManager.Instance.OnShopStarted += OnShopStarted;
        ShopManager.Instance.OnShopEnded += OnShopEnded;
    }

    private void OnRoundStarted(int obj)
    {
        TriggerAvailityDice(AvailityTriggerType.RoundStarted);
    }

    private void OnRoundClearStarted()
    {
        TriggerAvailityDice(AvailityTriggerType.RoundCleared);
    }

    private void OnShopStarted()
    {
        TriggerAvailityDice(AvailityTriggerType.ShopStarted);
    }

    private void OnShopEnded()
    {
        TriggerAvailityDice(AvailityTriggerType.ShopEnded);
    }
    #endregion

    #region TriggerDice
    public void TriggerPlayDices()
    {
        var playDiceList = PlayerDiceManager.Instance.GetOrderedPlayDiceList();
        var UsableDiceValues = PlayerDiceManager.Instance.UsableDiceValues;

        foreach (var playDice in playDiceList)
        {
            if (UsableDiceValues != null && !UsableDiceValues.Contains(playDice.DiceValue)) continue;

            playDice.ApplyScorePairs();
            TriggerAvailityDice(playDice);
        }
    }

    private void TriggerAvailityDice(AvailityTriggerType triggerType, AvailityDiceContext context)
    {
        List<AvailityDice> triggeredAvailityDiceList = PlayerDiceManager.Instance.AvailityDiceList.FindAll(dice => dice.IsTriggered(triggerType, context));

        foreach (var availityDice in triggeredAvailityDiceList)
        {
            availityDice.TriggerEffect();
        }
    }

    private void TriggerAvailityDice(AvailityTriggerType triggerType)
    {
        TriggerAvailityDice(triggerType, new());
    }

    private void TriggerAvailityDice(PlayDice playDice)
    {
        TriggerAvailityDice(AvailityTriggerType.PlayDice, new(playDice: playDice));
    }

    public void TriggerAvailityDice(HandSO handSO)
    {
        TriggerAvailityDice(AvailityTriggerType.Hand, new(handSO: handSO));
    }

    public void TriggerChaosDices()
    {
        foreach (var chaosDice in PlayerDiceManager.Instance.ChaosDiceList)
        {
            chaosDice.ApplyScorePairs();
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

        PlayerMoneyManager.Instance.Money += money;
        TriggerAnimationManager.Instance.PlayTriggerAnimation(targetTransform, offset, money);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }
    #endregion
}
