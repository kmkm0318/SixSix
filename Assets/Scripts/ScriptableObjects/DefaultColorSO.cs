using System.Collections.Generic;
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
    public Color gray;

    private Dictionary<string, string> colorMap;

    private void OnEnable()
    {
        colorMap = new Dictionary<string, string>
        {
            {"blue", ColorToHex(blue) },
            {"red", ColorToHex(red) },
            {"green", ColorToHex(green) },
            {"yellow", ColorToHex(yellow) },
            {"cyan", ColorToHex(cyan) },
            {"magenta", ColorToHex(magenta) },
            {"green_dark", ColorToHex(green_dark) },
            {"gray", ColorToHex(gray) },
        };
    }

    private string ColorToHex(Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGB(color);
    }

    public string FormatColor(string value)
    {
        if (colorMap == null || colorMap.Count == 0)
        {
            OnEnable();
        }

        foreach (var pair in colorMap)
        {
            value = value.Replace($"<color={pair.Key}>", $"<color={pair.Value}>");
        }

        return value;
    }
}
