using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class RoundClearUI : Singleton<RoundClearUI>
{
    [SerializeField] private RectTransform roundClearPanel;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private Button closeButton;
    [SerializeField] private FadeCanvasGroup fadeCanvasGroup;
    [SerializeField] private AnimatedText resultText;
    [SerializeField] private LocalizedString resultString;
    [SerializeField] private List<RewardTextPair> rewardTexts;

    private bool isClosable = false;
    private string resultTextValue = string.Empty;

    public event Action OnRoundClearUIOpened;
    public event Action OnRoundClearUIClosed;

    private void Start()
    {
        RegisterEvents();
        closeButton.onClick.AddListener(() =>
        {
            if (!isClosable) return;
            SequenceManager.Instance.AddCoroutine(Hide());
        });
        gameObject.SetActive(false);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.RoundClear, OnRoundClearStarted);
    }

    private void OnRoundClearStarted()
    {
        ClearTexts();
        SequenceManager.Instance.AddCoroutine(Show());
        SequenceManager.Instance.AddCoroutine(ShowTextAnimations());
        SequenceManager.Instance.AddCoroutine(() => isClosable = true);
    }
    #endregion

    #region ShowHide
    private IEnumerator Show()
    {
        gameObject.SetActive(true);
        roundClearPanel.anchoredPosition = hidePos;

        var myTween = roundClearPanel
            .DOAnchorPos(Vector3.zero, AnimationFunction.DefaultDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {

            });

        fadeCanvasGroup.FadeIn(AnimationFunction.DefaultDuration);

        yield return myTween.WaitForCompletion();
        OnRoundClearUIOpened?.Invoke();
    }

    private IEnumerator Hide()
    {
        if (!isClosable) yield break;
        isClosable = false;

        roundClearPanel.anchoredPosition = Vector3.zero;

        var myTween = roundClearPanel
             .DOAnchorPos(hidePos, AnimationFunction.DefaultDuration)
             .SetEase(Ease.InOutBack)
             .OnComplete(() =>
             {
                 gameObject.SetActive(false);
             });

        fadeCanvasGroup.FadeOut(AnimationFunction.DefaultDuration);

        yield return myTween.WaitForCompletion();
        OnRoundClearUIClosed?.Invoke();
    }
    #endregion

    #region TextAnimation
    private void ClearTexts()
    {
        resultTextValue = string.Empty;
        resultText.ClearText();
    }

    private IEnumerator ShowTextAnimations()
    {
        resultString.Arguments = new object[]
        {
            RoundManager.Instance.CurrentRound,
            UtilityFunctions.FormatNumber(ScoreManager.Instance.TargetRoundScore),
            UtilityFunctions.FormatNumber(ScoreManager.Instance.PreviousRoundScore),
        };
        resultString.RefreshString();
        resultTextValue = resultString.GetLocalizedString();

        ApplyReward();

        yield return resultText.ShowTextCoroutine(resultTextValue);
    }

    private void ApplyReward()
    {
        foreach (var type in RoundClearManager.Instance.DefaultRewardList)
        {
            int rewardValue = RoundClearManager.Instance.GetRewardValue(type);
            if (rewardValue <= 0) continue;

            var rewardUI = rewardTexts.Find(pair => pair.RewardType == type);
            if (rewardUI == null) continue;

            var args = type == RoundClearRewardType.MoneyInterest ?
            new object[] { GetRewardValueText(rewardValue), MoneyManager.Instance.InterestMax } :
            new object[] { GetRewardValueText(rewardValue) };

            rewardUI.RewardText.Arguments = args;
            rewardUI.RewardText.RefreshString();

            resultTextValue += "\n\n" + rewardUI.RewardText.GetLocalizedString();

            MoneyManager.Instance.AddMoney(rewardValue, true);
        }
    }

    private string GetRewardValueText(int value)
    {
        string text = string.Empty;
        for (int i = 0; i < value; i++)
        {
            text += "$";
        }
        return text;
    }
    #endregion
}

[Serializable]
public class RewardTextPair
{
    public RoundClearRewardType RewardType;
    public LocalizedString RewardText;

    public RewardTextPair(RoundClearRewardType rewardType, LocalizedString rewardText)
    {
        RewardType = rewardType;
        RewardText = rewardText;
    }
}