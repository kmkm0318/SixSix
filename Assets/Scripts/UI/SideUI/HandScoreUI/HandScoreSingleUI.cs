using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class HandScoreSingleUI : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent nameLocalizedText;
    [SerializeField] private TMP_Text enhanceLevelText;
    [SerializeField] private TMP_Text baseScoreText;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private Button _button;

    private HandSO _handSO;
    private Action<HandSO> _onClick;

    private void Awake()
    {
        _button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(SFXType.ButtonDown);
            _onClick?.Invoke(_handSO);
        });
    }

    public void Init(HandSO handSO, Action<HandSO> onClick)
    {
        nameLocalizedText.StringReference = handSO.handNameLocalized;

        _handSO = handSO;
        _onClick = onClick;

        ResetScoreText();
    }

    public void UpdateScoreText(ScorePair scorePair)
    {
        baseScoreText.text = UtilityFunctions.FormatNumber(scorePair.baseScore);
        multiplierText.text = UtilityFunctions.FormatNumber(scorePair.multiplier);

        baseScoreText.color = scorePair.baseScore > 0 ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);
        multiplierText.color = scorePair.multiplier > 0 ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);
    }

    public void ResetScoreText()
    {
        UpdateScoreText(new(0, 0));
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
