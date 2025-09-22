using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusUI : Singleton<BonusUI>
{
    [SerializeField] private List<Image> diceImage;
    [SerializeField] private List<AnimatedText> diceCountTextList;
    [SerializeField] private Transform bonusTargetTextParent;
    [SerializeField] private DiceTotalSum bonusTargetTextPrefab;

    private Dictionary<BonusType, TMP_Text> bonusTargetTextDict = new();

    private void Start()
    {
        InitDiceImages();
        InitBonusData();
        InitTexts();
        RegisterEvents();
    }

    private void InitDiceImages()
    {
        var diceSprite = DataContainer.Instance.DefaultDiceSpriteList.spriteList;
        for (int i = 0; i < diceImage.Count; i++)
        {
            if (i < diceSprite.Count)
            {
                diceImage[i].sprite = diceSprite[i];
            }
            else
            {
                Debug.LogError($"Dice image index {i} is out of range. Please check the sprite list.");
            }
        }
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
            diceCountTextList[i].SetText("0");
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

        AddTextAnimation(diceCountTextList[diceValue - 1].TMP_Text, sumValue.ToString());
    }

    private void OnTotalDiceSumChanged(BonusType type, int score)
    {
        if (bonusTargetTextDict.TryGetValue(type, out var targetText))
        {
            int targetScore = BonusManager.Instance.BonusTargetScoreDict[type];

            string targetString;

            targetString = score.ToString() + "/" + targetScore.ToString();
            AddTextAnimation(targetText, targetString);
        }
    }

    private void OnBonusAchieved(BonusType type)
    {
        if (bonusTargetTextDict.TryGetValue(type, out var targetText))
        {
            AddTextAnimation(targetText, "Successed");
        }
    }

    private void AddTextAnimation(TMP_Text text, string targetString)
    {
        SequenceManager.Instance.AddCoroutine(SetTextAndPlayAnimation(text, targetString), true);
        SequenceManager.Instance.AddCoroutine(() => AudioManager.Instance.PlaySFX(SFXType.DiceTrigger), true);
    }


    private IEnumerator SetTextAndPlayAnimation(TMP_Text targetText, string targetString)
    {
        targetText.text = targetString;
        yield return StartCoroutine(AnimationFunction.ShakeAnimation(targetText.transform));
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