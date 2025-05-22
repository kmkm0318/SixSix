using UnityEngine;

public class TextPanel : BasePanel
{
    [SerializeField] protected AnimatedText text;
    public AnimatedText Text => text;

    public void SetText(string textValue, bool isShow = false)
    {
        if (isShow)
        {
            text.ShowText(textValue);
        }
        else
        {
            text.SetText(textValue);
        }
    }
}