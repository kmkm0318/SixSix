using System;

public static class TextAnimationFunction
{
    private static string GetEffectText(string innerText, string effectType, TextAnimationModifier modifier = null)
    {
        string modifierText = string.Empty;

        if (modifier != null)
        {
            if (modifier.a != -1) modifierText += $" a={modifier.a}";
            if (modifier.f != -1) modifierText += $" f={modifier.f}";
            if (modifier.w != -1) modifierText += $" w={modifier.w}";
            if (modifier.d != -1) modifierText += $" d={modifier.d}";
        }

        return $"<{effectType}{modifierText}>{innerText}</{effectType}>";
    }

    public static string GetEffectText(string innerText, TextAnimationEffectType effectType, TextAnimationModifier modifier = null)
    {
        if (effectType == TextAnimationEffectType.None) return innerText;

        return GetEffectText(innerText, effectType.ToString(), modifier);
    }
}

public enum TextAnimationEffectType
{
    None,
    Bounce,
    Swing,
    Wave,
    Shake,
    Wiggle,
}

[Serializable]
public class TextAnimationModifier
{
    public float a = -1;
    public float f = -1;
    public float w = -1;
    public float d = -1;
}