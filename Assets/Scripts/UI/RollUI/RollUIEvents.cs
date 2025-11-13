using System;

public static class RollUIEvents
{
    public static event Action OnRollButtonPressed;
    public static event Action OnRollButtonReleased;

    public static void TriggerOnRollButtonPressed() => OnRollButtonPressed?.Invoke();
    public static void TriggerOnRollButtonReleased() => OnRollButtonReleased?.Invoke();
}