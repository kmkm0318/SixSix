using System;

public static class InteractInfoUIEvents
{
    public static event Action<DiceInteractionType, int> OnShowInteractInfoUI;
    public static event Action OnHideInteractInfoUI;

    public static void TriggerOnShowInteractInfoUI(DiceInteractionType interactType, int value)
    => OnShowInteractInfoUI?.Invoke(interactType, value);
    public static void TriggerOnHideInteractInfoUI()
    => OnHideInteractInfoUI?.Invoke();
}