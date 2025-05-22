using System.Collections;
using Febucci.UI;
using TMPro;
using UnityEngine;

public class AnimatedText : MonoBehaviour
{
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

        if (DataContainer.Instance != null && DataContainer.Instance.DefaultColorSO != null)
        {
            text = DataContainer.Instance.DefaultColorSO.FormatColor(text);
        }

        setText.SetText(text);
    }

    public void ShowText(string text)
    {
        CheckThenStopShowing();

        if (DataContainer.Instance != null && DataContainer.Instance.DefaultColorSO != null)
        {
            text = DataContainer.Instance.DefaultColorSO.FormatColor(text);
        }

        showText.ShowText(text);
    }

    public IEnumerator ShowTextCoroutine(string text)
    {
        CheckThenStopShowing();

        if (DataContainer.Instance != null && DataContainer.Instance.DefaultColorSO != null)
        {
            text = DataContainer.Instance.DefaultColorSO.FormatColor(text);
        }

        showText.ShowText(text);

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