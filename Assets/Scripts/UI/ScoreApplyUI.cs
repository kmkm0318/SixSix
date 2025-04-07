using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreApplyUI : Singleton<ScoreApplyUI>
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Color baseScoreColor;
    [SerializeField] private Color multiplierColor;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        Hide();
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        ScoreManager.Instance.OnScorePairApplied += OnScorePairApplied;
    }

    public void OnScorePairApplied(ScorePair pair, Transform targetTrasform)
    {
        SequenceManager.Instance.AddCoroutine(ShowScorePairTextAnimation(pair, targetTrasform), true);
        SequenceManager.Instance.AddCoroutine(AnimationManager.Instance.PlayAnimation(this, AnimationType.Shake), true);
    }

    private IEnumerator ShowScorePairTextAnimation(ScorePair pair, Transform targetTransform)
    {
        Show();

        SetupScorePairUI(pair, targetTransform);
        yield return StartCoroutine(AnimationManager.Instance.PlayAnimation(scoreText, AnimationType.Text));

        Hide();
    }

    private void SetupScorePairUI(ScorePair pair, Transform transform)
    {
        if (transform == null) return;

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

        this.transform.position = transform.position + offset;
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
