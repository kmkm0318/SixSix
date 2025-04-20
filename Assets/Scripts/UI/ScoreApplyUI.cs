using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreApplyUI : Singleton<ScoreApplyUI>
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Color baseScoreColor;
    [SerializeField] private Color multiplierColor;
    [SerializeField] private Color moneyColor;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform moneyTextPos;
    [SerializeField] private Vector3 moneyTextOffset;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Hide();
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        ScoreManager.Instance.OnScorePairApplied += OnScorePairApplied;
        ScoreManager.Instance.OnMoneyAchieved += OnMoneyAchieved;
    }

    private void OnMoneyAchieved(int money, Transform transform, bool isAvailityDice)
    {
        if (money == 0) return;

        SequenceManager.Instance.AddCoroutine(MoneyAnimation(money, transform, isAvailityDice), true);
    }

    private IEnumerator MoneyAnimation(int money, Transform transform, bool isAvailityDice)
    {
        Show();

        SetupMoneyUI(money);
        SetPosition(transform, isAvailityDice);

        yield return StartCoroutine(AnimationManager.Instance.PlayShakeAnimation(scoreText.transform));

        Hide();
    }

    private void SetupMoneyUI(int money)
    {
        scoreText.color = moneyColor;
        scoreText.text = money > 0 ? "+$" : "-$";
        scoreText.text += Math.Abs(money).ToString();
    }

    public void OnScorePairApplied(ScorePair pair, Transform targetTransform, bool isAvilityDice)
    {
        SequenceManager.Instance.AddCoroutine(ShowScorePairTextAnimation(pair, targetTransform, isAvilityDice), true);
    }

    private IEnumerator ShowScorePairTextAnimation(ScorePair pair, Transform targetTransform, bool isAvilityDice)
    {
        Show();

        SetupScorePairUI(pair);
        SetPosition(targetTransform, isAvilityDice);

        yield return StartCoroutine(AnimationManager.Instance.PlayShakeAnimation(scoreText.transform));

        Hide();
    }

    private void SetupScorePairUI(ScorePair pair)
    {
        bool isBaseScore = pair.baseScore != 0;
        bool isMultiplier = pair.multiplier != 0;

        if (!isBaseScore && !isMultiplier) return;

        if (isBaseScore)
        {
            scoreText.color = baseScoreColor;
            scoreText.text = "+" + pair.baseScore.ToString();
        }
        else if (isMultiplier)
        {
            scoreText.color = multiplierColor;
            scoreText.text = "x" + pair.multiplier.ToString();
        }
    }

    private void SetPosition(Transform targetTransform, bool isAvailityDice)
    {
        if (targetTransform == null) return;

        Vector3 targetPos;

        if (isAvailityDice)
        {
            targetPos = targetTransform.position - offset / 2f;
        }
        else
        {
            targetPos = targetTransform.position + offset;
        }

        var uiPos = Camera.main.WorldToScreenPoint(targetPos);

        rectTransform.position = uiPos;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        scoreText.text = "";
        gameObject.SetActive(false);
    }
}
