using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusUI : Singleton<BonusUI>
{
    [SerializeField] private List<TMP_Text> diceCountTextList;
    [SerializeField] private Transform bonusTargetTextParent;
    [SerializeField] private BonusTargetText bonusTargetTextPrefab;

    private Dictionary<BonusType, TMP_Text> bonusTargetTextDict = new();

    private void Start()
    {
        InitBonusData();
        InitTexts();
        RegisterEvents();
    }

    private void InitBonusData()
    {
        foreach (var bonusPair in BonusManager.Instance.BonusTargetScoreDict)
        {
            var newText = Instantiate(bonusTargetTextPrefab, bonusTargetTextParent);
            newText.gameObject.SetActive(true);
            newText.Text.text = "0/" + bonusPair.Value.ToString();
            bonusTargetTextDict[bonusPair.Key] = newText.Text;
        }
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

        yield return StartCoroutine(AnimationFunction.PlayShakeAnimation(targetText.transform));
    }
}

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