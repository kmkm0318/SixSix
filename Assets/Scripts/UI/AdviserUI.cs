using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class AdviserUI : Singleton<AdviserUI>
{
    [SerializeField] private RectTransform advisePanel;
    [SerializeField] private TMP_Text adviseText;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private float showTime;
    [SerializeField] private float adviseMinProbability;

    private Tween currentTween;
    private bool isAvailiable = true;

    private void Start()
    {
        RegisterEvents();
        advisePanel.gameObject.SetActive(false);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        RollManager.Instance.OnRollCompleted += OnRollCompleted;
        EnhanceManager.Instance.OnDiceEnhanceStarted += OnDiceEnhanceStarted;
        EnhanceManager.Instance.OnDiceEnhanceCompleted += OnDiceEnhanceCompleted;
    }

    private void OnRollCompleted()
    {
        if (RollManager.Instance.RollRemain == 0 || !isAvailiable) return;

        ShowAdvise();
    }

    private void OnDiceEnhanceStarted()
    {
        isAvailiable = false;
    }

    private void OnDiceEnhanceCompleted()
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

        foreach (var hand in sortedHandList)
        {
            Debug.Log($"{hand.Item1} : {hand.Item2:P2} : {hand.Item3.baseScore * hand.Item3.multiplier}");
        }

        var adviseString = GetAdviseString(sortedHandList);
        if (adviseString == string.Empty) return;

        adviseText.text = adviseString;
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
                string handName = DataContainer.Instance.GetHandSO(hand.Item1).handName;
                res += $"{handName} : {probability:P2}";
            }
        }

        return res;
    }

    private void DelayHide(float delayTime)
    {
        currentTween?.Kill();

        currentTween = DOVirtual.DelayedCall(delayTime, () =>
        {
            Hide();
        });
    }
    #endregion

    private void Show(Action onComplete = null)
    {
        currentTween?.Kill();

        advisePanel.gameObject.SetActive(true);
        advisePanel.anchoredPosition = hidePos;

        currentTween = advisePanel
        .DOAnchorPos(Vector3.zero, DataContainer.Instance.DefaultDuration)
        .SetEase(Ease.InOutBack)
        .OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    private void Hide(Action onComplete = null)
    {
        currentTween?.Kill();

        advisePanel.anchoredPosition = Vector3.zero;

        currentTween = advisePanel
        .DOAnchorPos(hidePos, DataContainer.Instance.DefaultDuration)
        .SetEase(Ease.InOutBack)
        .OnComplete(() =>
        {
            advisePanel.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }
}