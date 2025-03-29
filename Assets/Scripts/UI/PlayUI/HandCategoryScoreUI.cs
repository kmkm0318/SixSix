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

    private Dictionary<HandCategory, HandCategoryScoreSingleUI> handCategoryScoreSingleUIDict = new();

    private void Start()
    {
        Init();
        ScoreManager.Instance.OnHandCategoryScoreUpdated += OnHandCategoryScoreUpdated;

        Invoke(nameof(ScrollLayoutPanel), 1f);
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

    private void OnHandCategoryScoreUpdated(Dictionary<HandCategory, ScorePair> dictionary)
    {
        foreach (var pair in dictionary)
        {
            handCategoryScoreSingleUIDict.TryGetValue(pair.Key, out var handCategoryScoreSingleUI);
            handCategoryScoreSingleUI.UpdateScore(pair.Value);
        }
    }

    public void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutPanel);
    }

    private void ScrollLayoutPanel()
    {
        layoutPanel.DOAnchorPos(targetScrollAnchoredPosition, scrollDuration).SetEase(Ease.InOutQuint);
    }
}
