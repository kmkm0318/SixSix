using UnityEngine;

public class TextPanel : BasePanel
{
    [SerializeField] protected AnimatedText _text;
    public AnimatedText Text => _text;

    public void SetText(string textValue, bool isShow = false)
    {
        if (isShow)
        {
            _text.ShowText(textValue);
        }
        else
        {
            _text.SetText(textValue);
        }
    }
}