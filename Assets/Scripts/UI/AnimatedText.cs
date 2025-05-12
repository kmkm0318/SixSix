using Febucci.UI;
using UnityEngine;

public class AnimatedText : MonoBehaviour
{
    private TextAnimator_TMP setText;
    private TypewriterByCharacter showText;

    private void Awake()
    {
        setText = GetComponent<TextAnimator_TMP>();
        showText = GetComponent<TypewriterByCharacter>();
    }

    #region Set/Show Text
    public void SetText(string text)
    {
        CheckThenStopShowing();

        setText.SetText(text);
    }

    public void ShowText(string text)
    {
        CheckThenStopShowing();

        showText.ShowText(text);
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