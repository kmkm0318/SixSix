using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreApplyUI : Singleton<ScoreApplyUI>
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Color baseScoreColor;
    [SerializeField] private Color multiplierColor;
    [SerializeField] private Vector3 offset;

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
    }

    public void OnScorePairApplied(ScorePair pair, Transform targetTransform, bool isAvilityDice)
    {
        SequenceManager.Instance.AddCoroutine(ShowScorePairTextAnimation(pair, targetTransform, isAvilityDice), true);
        SequenceManager.Instance.AddCoroutine(AnimationManager.Instance.PlayShakeAnimation(transform), true);
    }

    private IEnumerator ShowScorePairTextAnimation(ScorePair pair, Transform targetTransform, bool isAvilityDice)
    {
        Show();

        SetupScorePairUI(pair, targetTransform, isAvilityDice);
        yield return StartCoroutine(AnimationManager.Instance.PlayShakeAnimation(scoreText.transform));

        Hide();
    }

    private void SetupScorePairUI(ScorePair pair, Transform targetTransform, bool isAvilityDice)
    {
        if (targetTransform == null) return;

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

        Vector3 targetPos;

        if (isAvilityDice)
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
