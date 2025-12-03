using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class RoundClearUI : BaseUI
{
    [SerializeField] private Button closeButton;
    [SerializeField] private AnimatedText resultText;
    [SerializeField] private LocalizedString resultString;
    [SerializeField] private List<RewardTextPair> rewardTexts;

    private bool _isClosable = false;
    private string _resultTextValue = string.Empty;

    private void Start()
    {
        RegisterEvents();
        closeButton.onClick.AddListener(() =>
        {
            if (!_isClosable) return;
            Hide(RoundClearUIEvents.TriggerOnRoundClearUIHidden);
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
        SequenceManager.Instance.AddCoroutine(() =>
        {
            AudioManager.Instance.PlaySFX(SFXType.RoundClear);
            Show(RoundClearUIEvents.TriggerOnRoundClearUIShown);
        });
        SequenceManager.Instance.AddCoroutine(ShowTextAnimations());
        SequenceManager.Instance.AddCoroutine(() => _isClosable = true);
    }
    #endregion

    #region TextAnimation
    private void ClearTexts()
    {
        _resultTextValue = string.Empty;
        resultText.ClearText();
    }

    private IEnumerator ShowTextAnimations()
    {
        resultString.Arguments = new object[]
        {
            RoundManager.Instance.CurrentRound,
            UtilityFunctions.FormatNumber(ScoreManager.Instance.TargetRoundScore),
            UtilityFunctions.FormatNumber(ScoreManager.Instance.CurrentRoundScore),
        };
        resultString.RefreshString();
        _resultTextValue = resultString.GetLocalizedString();

        ApplyReward();

        yield return resultText.ShowTextCoroutine(_resultTextValue);
    }

    private void ApplyReward()
    {
        var playerStat = DataContainer.Instance.CurrentPlayerStat;

        foreach (var type in RoundClearManager.Instance.DefaultRewardList)
        {
            int rewardValue = RoundClearManager.Instance.GetRewardValue(type);
            if (rewardValue <= 0) continue;

            var rewardUI = rewardTexts.Find(pair => pair.RewardType == type);
            if (rewardUI == null) continue;

            var args = type == RoundClearRewardType.MoneyInterest ?
            new object[] { GetRewardValueText(rewardValue), playerStat.interestMax } :
            new object[] { GetRewardValueText(rewardValue) };

            rewardUI.RewardText.Arguments = args;
            rewardUI.RewardText.RefreshString();

            _resultTextValue += "\n\n" + rewardUI.RewardText.GetLocalizedString();

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