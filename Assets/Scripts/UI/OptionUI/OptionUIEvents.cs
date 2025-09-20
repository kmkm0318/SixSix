using System;

public static class OptionUIEvents
{
    public static event Action<OptionType, int> OnOptionValueChanged;
    public static event Action OnOptionButtonClicked;

    public static void TriggerOnOptionValueChanged(OptionType type, int value) => OnOptionValueChanged?.Invoke(type, value);
    public static void TriggerOnOptionButtonClicked() => OnOptionButtonClicked?.Invoke();
}