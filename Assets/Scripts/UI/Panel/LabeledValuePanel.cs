using UnityEngine;

public class LabeledValuePanel : BasePanel
{
    [SerializeField] private AnimatedText labelText;
    [SerializeField] private TextPanel valueTextPanel;

    public void SetLabel(string label, bool isShow = false)
    {
        if (isShow)
        {
            labelText.ShowText(label);
        }
        else
        {
            labelText.SetText(label);
        }
    }

    public void SetValue(string value, bool isShow = false)
    {
        valueTextPanel.SetText(value, isShow);
    }
}