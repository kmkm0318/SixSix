using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TriggerContextUI : Singleton<TriggerContextUI>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AnimatedText scoreText;
    [SerializeField] private float offsetDistance;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    #region ShowContext
    private IEnumerator ShowContext(Transform targetTransform, Vector3 offset, Action setupAction)
    {
        Show();

        setupAction();
        SetPosition(targetTransform, offset);

        yield return StartCoroutine(AnimationFunction.ShakeAnimation(scoreText.transform));

        Hide();
    }

    public IEnumerator ShowContext(Transform targetTransform, Vector3 offset, ScorePair pair)
    {
        return ShowContext(targetTransform, offset, () => SetupUI(pair));
    }

    public IEnumerator ShowContext(Transform targetTransform, Vector3 offset, int money)
    {
        return ShowContext(targetTransform, offset, () => SetupUI(money));
    }
    #endregion

    #region SetupUI
    private void SetupUI(ScorePair pair)
    {
        bool isBaseScore = pair.baseScore != 0;
        bool isMultiplier = pair.multiplier != 0;

        if (!isBaseScore && !isMultiplier) return;

        if (isBaseScore)
        {
            scoreText.TMP_Text.color = DataContainer.Instance.DefaultColorSO.blue;
            scoreText.SetText(pair.baseScore.ToString("+0;-0;0"));
        }
        else if (isMultiplier)
        {
            scoreText.TMP_Text.color = DataContainer.Instance.DefaultColorSO.red;
            scoreText.SetText("x" + pair.multiplier.ToString("0.##"));
        }
    }

    private void SetupUI(int money)
    {
        scoreText.TMP_Text.color = DataContainer.Instance.DefaultColorSO.yellow;
        string sign = money > 0 ? "+" : "-";
        int absMoney = Mathf.Abs(money);
        scoreText.SetText($"{sign}${absMoney}");
    }
    #endregion

    private void SetPosition(Transform targetTransform, Vector3 offset)
    {
        if (targetTransform == null) return;

        Vector3 targetPos = targetTransform.position + offsetDistance * targetTransform.localScale.x * offset;

        var uiPos = mainCamera.WorldToScreenPoint(targetPos);

        rectTransform.position = uiPos;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
