using Febucci.UI;
using UnityEngine;

public class AnimatedText : MonoBehaviour
{
    [Header("Awake Animation")]
    [SerializeField] private TextAnimationAwakeType awakeType = TextAnimationAwakeType.None;
    [SerializeField][TextArea(3, 10)] private string awakeText = string.Empty;

    [Header("Default Animation")]
    [SerializeField] private TextAnimationEffectType effectType;
    [SerializeField] private TextAnimationModifier modifier;

    private TextAnimator_TMP setText;
    private TypewriterByCharacter showText;

    private void Awake()
    {
        setText = GetComponent<TextAnimator_TMP>();
        showText = GetComponent<TypewriterByCharacter>();

        if (awakeType == TextAnimationAwakeType.SetText)
        {
            SetText(awakeText);
        }
        else if (awakeType == TextAnimationAwakeType.ShowText)
        {
            ShowText(awakeText);
        }
    }

    #region Set/Show Text
    public void SetText(string text, bool isDefault = false)
    {
        CheckThenStopShowing();

        if (isDefault)
        {
            text = TextAnimationFunction.GetEffectText(text, effectType, modifier);
        }

        setText.SetText(text);
    }

    public void SetText(string text, TextAnimationEffectType effectType, TextAnimationModifier modifier = null)
    {
        SetText(TextAnimationFunction.GetEffectText(text, effectType, modifier));
    }

    public void ShowText(string text, bool isDefault = false)
    {
        CheckThenStopShowing();

        if (isDefault)
        {
            text = TextAnimationFunction.GetEffectText(text, effectType, modifier);
        }

        showText.ShowText(text);
    }

    public void ShowText(string text, TextAnimationEffectType effectType, TextAnimationModifier modifier = null)
    {
        ShowText(TextAnimationFunction.GetEffectText(text, effectType, modifier));
    }
    #endregion

    private void CheckThenStopShowing()
    {
        if (showText.isShowingText)
        {
            showText.StopShowingText();
        }
    }
}

public enum TextAnimationAwakeType
{
    None,
    SetText,
    ShowText,
}