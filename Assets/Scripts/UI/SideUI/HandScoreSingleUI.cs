using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScoreSingleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text enhanceLevelText;
    [SerializeField] private TMP_Text baseScoreText;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private Button button;
    [SerializeField] private Color focusedColor;
    [SerializeField] private Color unfocusedColor;

    private ScorePair scorePair;
    private HandSO handSO;
    public HandSO HandSO => handSO;
    private int enhanceLevel = 0;
    public int EnhanceLevel => enhanceLevel;

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

    public void Init(HandSO handSO)
    {
        this.handSO = handSO;

        nameText.text = handSO.handName;

        UpdateScore(true);

        OnUnfocused();

        button.onClick.AddListener(() =>
        {
            if (scorePair.baseScore == 0 && scorePair.multiplier == 0) return;

            HandScoreUI.Instance.SelectHand(handSO, scorePair);
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
            scorePair = GetEnhancedSocrePair();
        }

        baseScoreText.text = UtilityFunctions.FormatNumber(scorePair.baseScore);
        multiplierText.text = UtilityFunctions.FormatNumber(scorePair.multiplier);
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
        StartCoroutine(AnimationFunction.PlayShakeAnimation(enhanceLevelText.transform));
        StartCoroutine(AnimationFunction.PlayShakeAnimation(baseScoreText.transform));
        StartCoroutine(AnimationFunction.PlayShakeAnimation(multiplierText.transform));

        UpdateScore(scorePair.baseScore == 0 && scorePair.multiplier == 0);
    }

    public ScorePair GetEnhancedSocrePair()
    {
        return handSO.GetEnhancedScorePair(enhanceLevel);
    }
}
