using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class HandScoreSingleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private LocalizeStringEvent nameLocalizedText;
    [SerializeField] private TMP_Text enhanceLevelText;
    [SerializeField] private TMP_Text baseScoreText;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private Button button;
    [SerializeField] private Color focusedColor;
    [SerializeField] private Color unfocusedColor;

    public void Init(HandSO handSO)
    {
        nameLocalizedText.StringReference = handSO.handNameLocalized;

        ResetScoreText();

        OnUnfocused();

        button.onClick.AddListener(() =>
        {
            HandScoreUI.Instance.HandleSelectHand(handSO);
        });
    }

    public void UpdateScoreText(ScorePair scorePair)
    {
        baseScoreText.text = UtilityFunctions.FormatNumber(scorePair.baseScore);
        multiplierText.text = UtilityFunctions.FormatNumber(scorePair.multiplier);
    }

    public void ResetScoreText()
    {
        UpdateScoreText(new(0, 0));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnFocused();
    }

    private void OnFocused()
    {
        baseScoreText.color = focusedColor;
        multiplierText.color = focusedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnUnfocused();
    }

    private void OnUnfocused()
    {
        baseScoreText.color = unfocusedColor;
        multiplierText.color = unfocusedColor;
    }

    public void PlayTriggerAnimation(int enhanceLevel, ScorePair scorePair)
    {
        enhanceLevelText.text = enhanceLevel.ToString();
        UpdateScoreText(scorePair);

        StartCoroutine(AnimationFunction.ShakeAnimation(enhanceLevelText.transform));
        StartCoroutine(AnimationFunction.ShakeAnimation(baseScoreText.transform));
        StartCoroutine(AnimationFunction.ShakeAnimation(multiplierText.transform));
    }
}
