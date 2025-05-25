using UnityEngine;

[CreateAssetMenu(fileName = "DefaultColorSO", menuName = "Scriptable Objects/DefaultColorSO")]
public class DefaultColorSO : ScriptableObject
{
    public Color blue;
    public Color red;
    public Color green;
    public Color yellow;
    public Color cyan;
    public Color magenta;
    public Color green_dark;

    public string FormatColor(string value)
    {
        return string.Format(
            value,
            ColorToHex(blue),
            ColorToHex(red),
            ColorToHex(green),
            ColorToHex(yellow)
        );
    }

    private string ColorToHex(Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGB(color);
    }
}
