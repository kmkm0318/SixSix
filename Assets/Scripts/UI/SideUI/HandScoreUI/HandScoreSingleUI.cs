using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;

public class HandScoreSingleUI : UIFocusHandler
{
    [SerializeField] private LocalizeStringEvent nameLocalizedText;
    [SerializeField] private TMP_Text enhanceLevelText;
    [SerializeField] private TMP_Text baseScoreText;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private Color focusedColor;
    [SerializeField] private Color unfocusedColor;

    private HandSO _handSO;
    private Action<HandSO> _onClick;

    private void Awake()
    {
        OnPointerEntered += OnFocused;
        OnPointerExited += OnUnfocused;
        OnPointerClicked += () =>
        {
            AudioManager.Instance.PlaySFX(SFXType.ButtonDown);
            _onClick?.Invoke(_handSO);
        };
    }

    public void Init(HandSO handSO, Action<HandSO> onClick)
    {
        nameLocalizedText.StringReference = handSO.handNameLocalized;

        _handSO = handSO;
        _onClick = onClick;

        ResetScoreText();
        OnUnfocused();
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

    private void OnFocused()
    {
        baseScoreText.color = focusedColor;
        multiplierText.color = focusedColor;
    }

    private void OnUnfocused()
    {
        baseScoreText.color = unfocusedColor;
        multiplierText.color = unfocusedColor;
    }

    public void PlayTriggerAnimation(int enhanceLevel, ScorePair scorePair)
    {
        SequenceManager.Instance.AddCoroutine(() => enhanceLevelText.text = enhanceLevel.ToString(), true);
        SequenceManager.Instance.AddCoroutine(() => UpdateScoreText(scorePair), true);
        AnimationFunction.AddShakeAnimation(enhanceLevelText.transform, true, true);
        AnimationFunction.AddShakeAnimation(baseScoreText.transform, true, true);
        AnimationFunction.AddShakeAnimation(multiplierText.transform, true, true);

        SequenceManager.Instance.ApplyParallelCoroutine();
    }
}
