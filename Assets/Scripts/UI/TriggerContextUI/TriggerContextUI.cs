using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class TriggerContextUI : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AnimatedText scoreText;
    [SerializeField] private float offsetDistance;
    [SerializeField] private LocalizedString retriggerText;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        RegisterEvents();
        Hide();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region Events
    private void RegisterEvents()
    {
        TriggerContextUIEvents.OnShowScoreContext += OnShowScoreContext;
        TriggerContextUIEvents.OnShowMoneyContext += OnShowMoneyContext;
        TriggerContextUIEvents.ShowValueContext += OnShowValueContext;
        TriggerContextUIEvents.ShowRetriggerContext += OnShowRetriggerContext;
    }

    private void UnregisterEvents()
    {
        TriggerContextUIEvents.OnShowScoreContext -= OnShowScoreContext;
        TriggerContextUIEvents.OnShowMoneyContext -= OnShowMoneyContext;
        TriggerContextUIEvents.ShowValueContext -= OnShowValueContext;
        TriggerContextUIEvents.ShowRetriggerContext -= OnShowRetriggerContext;
    }

    private void OnShowScoreContext(Transform targetTransform, Vector3 offset, ScorePair scorePair)
    {
        SequenceManager.Instance.AddCoroutine(ShowScoreContext(targetTransform, offset, scorePair), true);
    }

    private void OnShowMoneyContext(Transform targetTransform, Vector3 offset, int money)
    {
        SequenceManager.Instance.AddCoroutine(ShowMoneyContext(targetTransform, offset, money), true);
    }

    private void OnShowValueContext(Transform targetTransform, Vector3 offset, int value, string color)
    {
        SequenceManager.Instance.AddCoroutine(ShowValueContext(targetTransform, offset, value, color), true);
    }

    private void OnShowRetriggerContext(Transform targetTransform, Vector3 offset, string color)
    {
        SequenceManager.Instance.AddCoroutine(ShowRetriggerContext(targetTransform, offset, color), true);
    }
    #endregion

    #region ShowContext
    private IEnumerator ShowContext(Transform targetTransform, Vector3 offset, Action setupAction)
    {
        Show();

        setupAction();
        SetPosition(targetTransform, offset);

        yield return StartCoroutine(AnimationFunction.ShakeAnimation(scoreText.transform));

        Hide();
    }

    public IEnumerator ShowScoreContext(Transform targetTransform, Vector3 offset, ScorePair pair)
    {
        return ShowContext(targetTransform, offset, () => SetupScoreUI(pair));
    }

    public IEnumerator ShowMoneyContext(Transform targetTransform, Vector3 offset, int money)
    {
        return ShowContext(targetTransform, offset, () => SetupMoneyUI(money));
    }

    public IEnumerator ShowValueContext(Transform targetTransform, Vector3 offset, int value, string color)
    {
        return ShowContext(targetTransform, offset, () => SetupValueUI(value, color));
    }

    public IEnumerator ShowRetriggerContext(Transform targetTarnsform, Vector3 offset, string color)
    {
        return ShowContext(targetTarnsform, offset, () => SetupRetriggerUI(color));
    }
    #endregion

    #region SetupUI
    private void SetupScoreUI(ScorePair pair)
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

    private void SetupMoneyUI(int money)
    {
        scoreText.TMP_Text.color = DataContainer.Instance.DefaultColorSO.yellow;
        string sign = money > 0 ? "+" : "-";
        int absMoney = Mathf.Abs(money);
        scoreText.SetText($"{sign}${absMoney}");
    }

    private void SetupValueUI(int value, string color)
    {
        string res = "<color=" + color + ">" + value.ToString("+0;-0;0") + "</color>";
        scoreText.SetText(res);
    }

    private void SetupRetriggerUI(string color)
    {
        string res = "<color=" + color + ">" + retriggerText.GetLocalizedString() + "</color>";
        scoreText.SetText(res);
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
        scoreText.SetText(string.Empty);
        gameObject.SetActive(false);
    }
}
