using System;

public static class RoundClearUIEvents
{
    public static event Action OnRoundClearUIShown;
    public static event Action OnRoundClearUIHidden;

    public static void TriggerOnRoundClearUIShown() => OnRoundClearUIShown?.Invoke();
    public static void TriggerOnRoundClearUIHidden() => OnRoundClearUIHidden?.Invoke();
}