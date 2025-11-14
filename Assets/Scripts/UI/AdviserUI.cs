using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class AdviserUI : BaseUI
{
    [SerializeField] private LabeledValuePanel advisePanel;
    [SerializeField] private float showTime;
    [SerializeField] private float adviseMinProbability;

    private bool isAvailiable = true;

    private void Start()
    {
        RegisterEvents();
        gameObject.SetActive(false);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Roll, null, OnRollCompleted);

        GameManager.Instance.RegisterEvent(GameState.Enhance, OnEnhanceStarted, OnEnhanceCompleted);
    }

    private void OnRollCompleted()
    {
        if (RollManager.Instance.RollRemain == 0 || !isAvailiable) return;

        ShowAdvise();
    }

    private void OnEnhanceStarted()
    {
        isAvailiable = EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Hand;
    }

    private void OnEnhanceCompleted()
    {
        isAvailiable = true;
    }
    #endregion

    #region ShowAdvise
    private void ShowAdvise()
    {
        var handList = GetHandList();

        var sortedHandList = handList
        .OrderByDescending(x => x.Item2)
        .ThenByDescending(x => x.Item3.baseScore)
        .ToList();

        var adviseString = GetAdviseString(sortedHandList);
        if (adviseString == string.Empty) return;

        advisePanel.SetValue(adviseString);
        Show(() => DelayHide(showTime));
    }

    private static List<(Hand, float, ScorePair)> GetHandList()
    {
        var diceValues = DiceManager.Instance.PlayDiceList.Select(dice => dice.DiceValue).ToList();
        var handProbabilities = HandCalculator.GetHandProbabilities(diceValues);
        var handScorePairs = HandManager.Instance.HandScores;

        List<(Hand, float, ScorePair)> res = new();

        foreach (var hand in handProbabilities.Keys)
        {
            if (handScorePairs.ContainsKey(hand))
            {
                var scorePair = handScorePairs[hand];
                var probability = handProbabilities[hand];
                res.Add((hand, probability, scorePair));
            }
        }

        return res;
    }

    private string GetAdviseString(List<(Hand, float, ScorePair)> sortedHandList)
    {
        string res = string.Empty;
        if (sortedHandList.Count == 0) return res;

        for (int i = 0; i < sortedHandList.Count; i++)
        {
            var hand = sortedHandList[i];
            var probability = hand.Item2;

            if (probability == 1f) continue;
            if (probability < adviseMinProbability) continue;

            var scorePair = hand.Item3;
            if (scorePair.baseScore > sortedHandList.First().Item3.baseScore)
            {
                if (res.Length != 0) res += "\n";
                string handName = DataContainer.Instance.GetHandSO(hand.Item1).HandName;
                res += $"{handName} : {probability:P2}";
            }
        }

        return res;
    }

    private void DelayHide(float delayTime)
    {
        _currentTween?.Kill();

        _currentTween = DOVirtual.DelayedCall(delayTime, () =>
        {
            Hide();
        });
    }
    #endregion
}