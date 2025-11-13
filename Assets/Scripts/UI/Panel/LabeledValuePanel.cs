using UnityEngine;

public class LabeledValuePanel : BasePanel
{
    [SerializeField] private AnimatedText _labelText;
    [SerializeField] private TextPanel _valueTextPanel;

    public void SetLabel(string label, bool isShow = false)
    {
        if (isShow)
        {
            _labelText.ShowText(label);
        }
        else
        {
            _labelText.SetText(label);
        }
    }

    public void SetValue(string value, bool isShow = false)
    {
        _valueTextPanel.SetText(value, isShow);
    }
}