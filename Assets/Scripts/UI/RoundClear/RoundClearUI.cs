using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class RoundClearUI : Singleton<RoundClearUI>
{
    [SerializeField] private RectTransform roundClearPanel;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private Button closeButton;
    [SerializeField] private FadeCanvasGroup fadeCanvasGroup;
    [SerializeField] private AnimatedText resultText;
    [SerializeField] private Transform roundClearRewardParent;
    [SerializeField] private RoundClearRewardUI roundClearRewardUI;

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
        resultTextValue += $"<wave>Cleared Round</wave> : <bounce>{RoundManager.Instance.CurrentRound}</bounce>";
        resultTextValue += $"\n<wave>Target Score</wave> : <bounce>{UtilityFunctions.FormatNumber(ScoreManager.Instance.TargetRoundScore)}</bounce>";
        resultTextValue += $"\n<wave>Your Score</wave> : <bounce>{UtilityFunctions.FormatNumber(ScoreManager.Instance.PreviousRoundScore)}</bounce>";

        ApplyReward();

        yield return resultText.ShowTextCoroutine(resultTextValue);
    }

    private void ApplyReward()
    {
        foreach (var type in RoundClearManager.Instance.DefaultRewardList)
        {
            int rewardValue = RoundClearManager.Instance.GetRewardValue(type);
            if (rewardValue <= 0) continue;
            string left = $"<wave>{GetRewardName(type)}</wave>";
            string right = $"<bounce>{GetRewardValueText(rewardValue)}</bounce>";
            resultTextValue += $"\n{left} : {right}";
            MoneyManager.Instance.AddMoney(rewardValue, true);
        }
    }

    private string GetRewardName(RoundClearRewardType type)
    {
        return type switch
        {
            RoundClearRewardType.RoundClear => "Round Clear",
            RoundClearRewardType.PlayRemain => "Play Remain",
            RoundClearRewardType.MoneyInterest => "Money Interest",
            RoundClearRewardType.BossRoundClear => "Boss Round Clear",
            _ => string.Empty,
        };
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