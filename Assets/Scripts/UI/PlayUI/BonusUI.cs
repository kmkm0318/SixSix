using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct BonusTypeTextPair
{
    public BonusType type;
    public TMP_Text text;

    public BonusTypeTextPair(BonusType type, TMP_Text text)
    {
        this.type = type;
        this.text = text;
    }
}

public class BonusUI : Singleton<BonusUI>
{
    [SerializeField] private List<TMP_Text> diceCountTextList;
    [SerializeField] private List<BonusTypeTextPair> bonusTargetTextList;

    private Dictionary<BonusType, TMP_Text> bonusTargetTextDict = new();

    protected override void Awake()
    {
        base.Awake();

        foreach (var bonusTargetText in bonusTargetTextList)
        {
            bonusTargetTextDict[bonusTargetText.type] = bonusTargetText.text;
        }
    }

    private void Start()
    {
        InitTexts();
        RegisterEvents();
    }

    private void InitTexts()
    {
        for (int i = 0; i < diceCountTextList.Count; i++)
        {
            diceCountTextList[i].text = "0";
        }

        foreach (var bonusTargetTextPair in bonusTargetTextDict)
        {
            var bonusTargetText = bonusTargetTextPair.Value;
            var targetScore = BonusManager.Instance.BonusTargetScoreDict[bonusTargetTextPair.Key];
            bonusTargetText.text = "0/" + targetScore.ToString();
        }
    }

    private void RegisterEvents()
    {
        BonusManager.Instance.OnDiceSumChanged += OnDiceSumChanged;
        BonusManager.Instance.OnTotalDiceSumChanged += OnTotalDiceSumChanged;
        BonusManager.Instance.OnBonusAchieved += OnBonusAchieved;
    }

    private void OnDiceSumChanged(int diceValue, int sumValue)
    {
        if (diceValue <= 0 || diceValue > diceCountTextList.Count) return;

        SequenceManager.Instance.AddCoroutine(SetTextAndPlayAnimation(diceCountTextList[diceValue - 1], sumValue.ToString()), true);
    }

    private void OnTotalDiceSumChanged(BonusType type, int score)
    {
        if (bonusTargetTextDict.TryGetValue(type, out var targetText))
        {
            int targetScore = BonusManager.Instance.BonusTargetScoreDict[type];

            string targetString;

            targetString = score.ToString() + "/" + targetScore.ToString();

            SequenceManager.Instance.AddCoroutine(SetTextAndPlayAnimation(targetText, targetString), true);
        }
    }

    private void OnBonusAchieved(BonusType type)
    {
        if (bonusTargetTextDict.TryGetValue(type, out var targetText))
        {
            SequenceManager.Instance.AddCoroutine(SetTextAndPlayAnimation(targetText, "Successed"), true);
        }
    }

    private IEnumerator SetTextAndPlayAnimation(TMP_Text targetText, string targetString)
    {
        targetText.text = targetString;

        yield return StartCoroutine(AnimationManager.Instance.PlayAnimation(targetText, AnimationType.Shake));
    }
}