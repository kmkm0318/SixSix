using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HandCategoryScoreUI : Singleton<HandCategoryScoreUI>
{
    [SerializeField] private Transform handCategoryScoreSingleUIPrefab;
    [SerializeField] private RectTransform layoutPanel;
    [SerializeField] private float scrollDuration;
    [SerializeField] private Vector2 targetScrollAnchoredPosition;

    public event Action<HandCategorySO> OnHandCategorySelected;

    private Dictionary<HandCategory, HandCategoryScoreSingleUI> handCategoryScoreSingleUIDict = new();
    private bool isActive = false;

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        foreach (var handCategorySO in DataContainer.Instance.HandCategoryListSO.handCategoryList)
        {
            var handCategoryScoreSingleUITransform = Instantiate(handCategoryScoreSingleUIPrefab, layoutPanel);
            var handCategoryScoreSingleUI = handCategoryScoreSingleUITransform.GetComponent<HandCategoryScoreSingleUI>();
            handCategoryScoreSingleUI.Init(handCategorySO);
            handCategoryScoreSingleUIDict.Add(handCategorySO.handCategory, handCategoryScoreSingleUI);
        }

        RebuildLayout();
    }

    private void RegisterEvents()
    {
        ScoreManager.Instance.OnHandCategoryScoreUpdated += OnHandCategoryScoreUpdated;

        PlayManager.Instance.OnPlayStarted += OnPlayStarted;
        RoundManager.Instance.OnRoundCleared += OnRoundCleared;

        RollManager.Instance.OnRollCompleted += () => isActive = true;
        RollManager.Instance.OnRollStarted += () => isActive = false;

        BonusManager.Instance.OnAllBonusAchieved += OnAllBonusAchieved;
    }

    private void OnHandCategoryScoreUpdated(Dictionary<HandCategory, ScorePair> dictionary)
    {
        foreach (var pair in dictionary)
        {
            handCategoryScoreSingleUIDict.TryGetValue(pair.Key, out var handCategoryScoreSingleUI);
            handCategoryScoreSingleUI.UpdateScore(pair.Value);
        }
    }

    private void OnPlayStarted(int playRemain)
    {
        ResetHandCategoryScoreUI();
    }

    private void OnRoundCleared(int round)
    {
        ResetHandCategoryScoreUI();
    }

    private void OnAllBonusAchieved()
    {
        SequenceManager.Instance.AddCoroutine(() => { ScrollLayoutPanel(); });
    }

    public void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutPanel);
    }

    private void ScrollLayoutPanel()
    {
        layoutPanel.DOAnchorPos(targetScrollAnchoredPosition, scrollDuration).SetEase(Ease.InOutQuint);
    }

    public void SelectHandCategory(HandCategorySO handCategorySO)
    {
        if (!isActive) return;
        isActive = false;

        OnHandCategorySelected?.Invoke(handCategorySO);
    }

    private void ResetHandCategoryScoreUI()
    {
        foreach (var pair in handCategoryScoreSingleUIDict)
        {
            pair.Value.UpdateScore(new());
        }
        SequenceManager.Instance.ApplyParallelCoroutine();
    }
}
