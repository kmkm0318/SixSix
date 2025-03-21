using System.Collections.Generic;
using UnityEngine;

public class HandCategoryScoreUI : Singleton<HandCategoryScoreUI>
{
    [SerializeField] private Transform handCategoryScoreSingleUIPrefab;

    private Dictionary<HandCategory, HandCategoryScoreSingleUI> handCategoryScoreSingleUIDict = new();

    private void Start()
    {
        Init();
        ScoreManager.Instance.OnHandCategoryScoreUpdated += OnHandCategoryScoreUpdated;
    }

    private void Init()
    {
        foreach (var handCategorySO in DataContainer.Instance.HandCategoryListSO.handCategoryList)
        {
            var handCategoryScoreSingleUITransform = Instantiate(handCategoryScoreSingleUIPrefab, transform);
            var handCategoryScoreSingleUI = handCategoryScoreSingleUITransform.GetComponent<HandCategoryScoreSingleUI>();
            handCategoryScoreSingleUI.Init(handCategorySO);
            handCategoryScoreSingleUIDict.Add(handCategorySO.handCategory, handCategoryScoreSingleUI);
        }

        foreach (var specialHandCategorySO in DataContainer.Instance.SpecialHandCategoryListSO.handCategoryList)
        {
            handCategoryScoreSingleUIDict.TryGetValue(specialHandCategorySO.handCategory, out var handCategoryScoreSingleUI);
            handCategoryScoreSingleUI.gameObject.SetActive(false);
        }
    }

    private void OnHandCategoryScoreUpdated(Dictionary<HandCategory, ScorePair> dictionary)
    {
        foreach (var pair in dictionary)
        {
            handCategoryScoreSingleUIDict.TryGetValue(pair.Key, out var handCategoryScoreSingleUI);
            handCategoryScoreSingleUI.UpdateScore(pair.Value);
        }
    }

}
