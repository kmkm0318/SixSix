using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCategoryScoreSingleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text enhanceLevelText;
    [SerializeField] private TMP_Text baseScoreText;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private Button button;
    [SerializeField] private Color focusedColor;
    [SerializeField] private Color unfocusedColor;

    private ScorePair scorePair;
    private HandCategorySO handCategorySO;
    private int enhanceLevel = 0;

    private bool isActive = true;
    private bool IsActive
    {
        get => isActive;
        set
        {
            if (isActive == value) return;
            isActive = value;

            if (!value)
            {
                OnUnfocused();
            }
        }
    }

    public void Init(HandCategorySO handCategorySO)
    {
        this.handCategorySO = handCategorySO;

        nameText.text = handCategorySO.handCategoryName;

        UpdateScore(true);

        OnUnfocused();

        button.onClick.AddListener(() =>
        {
            if (scorePair.baseScore == 0 && scorePair.multiplier == 0) return;

            HandCategoryScoreUI.Instance.SelectHandCategory(handCategorySO, scorePair);
        });
    }

    public void UpdateScore(bool isZero)
    {
        if (isZero)
        {
            scorePair = new();
        }
        else
        {
            scorePair = handCategorySO.GetEnhancedScorePair(enhanceLevel);
        }

        baseScoreText.text = scorePair.baseScore.ToString();
        multiplierText.text = scorePair.multiplier.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsActive)
        {
            OnFocused();
        }
    }

    private void OnFocused()
    {
        baseScoreText.color = focusedColor;
        multiplierText.color = focusedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsActive)
        {
            OnUnfocused();
        }
    }

    private void OnUnfocused()
    {
        baseScoreText.color = unfocusedColor;
        multiplierText.color = unfocusedColor;
    }

    public void Enhance(int increaseAmount)
    {
        enhanceLevel += increaseAmount;

        enhanceLevelText.text = enhanceLevel.ToString();
        StartCoroutine(AnimationManager.Instance.PlayShakeAnimation(enhanceLevelText.transform));

        UpdateScore(scorePair.baseScore == 0 && scorePair.multiplier == 0);
    }
}
