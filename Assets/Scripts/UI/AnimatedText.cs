using System.Collections;
using Febucci.UI;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
[RequireComponent(typeof(TextAnimator_TMP))]
[RequireComponent(typeof(TypewriterByCharacter))]
public class AnimatedText : MonoBehaviour
{
    [SerializeField] private DefaultColorSO textColorSO;

    private TMP_Text tmp_text;
    private TextAnimator_TMP setText;
    private TypewriterByCharacter showText;

    public TMP_Text TMP_Text => tmp_text;

    private void Awake()
    {
        tmp_text = GetComponent<TMP_Text>();
        setText = GetComponent<TextAnimator_TMP>();
        showText = GetComponent<TypewriterByCharacter>();
    }

    #region Text
    public void ClearText()
    {
        SetText(string.Empty);
    }

    public void SetText(string text)
    {
        CheckThenStopShowing();
        text = textColorSO.FormatColor(text);
        setText.SetText(text);
    }

    public void ShowText(string text)
    {
        CheckThenStopShowing();
        text = textColorSO.FormatColor(text);
        showText.ShowText(text);
    }

    public IEnumerator ShowTextCoroutine(string text)
    {
        ShowText(text);
        yield return new WaitUntil(() => !showText.isShowingText);
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