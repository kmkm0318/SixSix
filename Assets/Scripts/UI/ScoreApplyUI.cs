using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreApplyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Color baseScoreColor;
    [SerializeField] private Color multiplierColor;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        RegisterEvents();
        Hide();
    }

    private void RegisterEvents()
    {
        ScoreManager.Instance.OnScoreApplied += OnScoreApplied;
    }

    private void OnScoreApplied(ScorePair pair, Transform targetTrasform)
    {
        SequenceManager.Instance.AddCoroutine(ShowTextAnimation(pair, targetTrasform), true);
        SequenceManager.Instance.AddCoroutine(AnimationManager.Instance.PlayAnimation(this, AnimationType.Shake), true);
    }

    private IEnumerator ShowTextAnimation(ScorePair pair, Transform targetTransform)
    {
        Show();

        SetupUI(pair, targetTransform);
        yield return StartCoroutine(AnimationManager.Instance.PlayAnimation(scoreText, AnimationType.Text));

        Hide();
    }

    private void SetupUI(ScorePair pair, Transform transform)
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
