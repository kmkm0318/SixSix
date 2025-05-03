using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HandScoreUI : Singleton<HandScoreUI>
{
    [SerializeField] private Transform handScoreSingleUIPrefab;
    [SerializeField] private RectTransform layoutPanel;
    [SerializeField] private float scrollDuration;
    [SerializeField] private Vector2 targetScrollAnchoredPosition;

    public event Action<HandSO, ScorePair> OnHandSelected;

    private Dictionary<Hand, HandScoreSingleUI> handScoreSingleUIDict = new();
    private bool isActive = false;

    private HandSO usableHandSO = null;
    public HandSO UsableHandSO
    {
        get => usableHandSO;
        set
        {
            if (usableHandSO == value) return;
            usableHandSO = value;
        }
    }

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        foreach (var handSO in DataContainer.Instance.TotalHandListSO.handList)
        {
            var handScoreSingleUITransform = Instantiate(handScoreSingleUIPrefab, layoutPanel);
            var handScoreSingleUI = handScoreSingleUITransform.GetComponent<HandScoreSingleUI>();
            handScoreSingleUI.Init(handSO);
            handScoreSingleUIDict.Add(handSO.hand, handScoreSingleUI);
        }

        RebuildLayout();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ScoreManager.Instance.OnHandScoreUpdated += OnHandScoreUpdated;

        PlayManager.Instance.OnPlayStarted += OnPlayStarted;

        RollManager.Instance.OnRollStarted += OnRollStarted;
        RollManager.Instance.OnRollCompleted += OnRollCompleted;

        BonusManager.Instance.OnAllBonusAchieved += OnAllBonusAchieved;

        EnhanceManager.Instance.OnDiceEnhanceStarted += OnDiceEnhanceStarted;
        EnhanceManager.Instance.OnDiceEnhanceCompleted += OnDiceEnhanceCompleted;
        EnhanceManager.Instance.OnHandEnhanceStarted += OnHandEnhanceStarted;
        EnhanceManager.Instance.OnHandEnhanceCompleted += OnHandEnhanceCompleted;
    }

    private void OnHandScoreUpdated(Dictionary<Hand, ScorePair> dictionary)
    {
        foreach (var pair in dictionary)
        {
            handScoreSingleUIDict.TryGetValue(pair.Key, out var handScoreSingleUI);
            handScoreSingleUI.UpdateScore(pair.Value.baseScore == 0 && pair.Value.multiplier == 0);
        }
    }

    private void OnPlayStarted(int playRemain)
    {
        ResetHandScoreUI();
    }

    private void OnRollStarted()
    {
        isActive = false;
    }

    private void OnRollCompleted()
    {
        isActive = true;
    }

    private void OnAllBonusAchieved()
    {
        SequenceManager.Instance.AddCoroutine(ScrollLayoutPanel);
    }

    private void OnDiceEnhanceStarted()
    {
        isActive = false;

        ScoreManager.Instance.OnHandScoreUpdated -= OnHandScoreUpdated;
    }

    private void OnDiceEnhanceCompleted()
    {
        isActive = false;

        ScoreManager.Instance.OnHandScoreUpdated += OnHandScoreUpdated;
    }

    private void OnHandEnhanceStarted()
    {
        isActive = true;
    }

    private void OnHandEnhanceCompleted()
    {
        isActive = false;
    }
    #endregion

    public void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutPanel);
    }

    private void ScrollLayoutPanel()
    {
        layoutPanel.DOAnchorPos(targetScrollAnchoredPosition, scrollDuration).SetEase(Ease.InOutQuint);
    }

    public void SelectHand(HandSO handSO, ScorePair scorePair)
    {
        if (usableHandSO != null && usableHandSO != handSO) return;
        if (!isActive) return;
        isActive = false;

        OnHandSelected?.Invoke(handSO, scorePair);
    }

    private void ResetHandScoreUI()
    {
        foreach (var pair in handScoreSingleUIDict)
        {
            pair.Value.UpdateScore(true);
        }
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public void EnhanceHand(HandSO handSO, int enhanceLevel)
    {
        if (handScoreSingleUIDict.TryGetValue(handSO.hand, out var handScoreSingleUI))
        {
            handScoreSingleUI.Enhance(enhanceLevel);
        }
    }
}
