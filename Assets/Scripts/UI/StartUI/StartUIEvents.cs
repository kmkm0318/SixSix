using System;
using System.Collections.Generic;

public static class StartUIEvents
{
    public static event Action OnStartUIButtonClicked;
    public static event Action<PlayerStatSO> OnPlayerStatSelected;

    public static void TriggerOnStartUIButtonClicked() => OnStartUIButtonClicked?.Invoke();
    public static void TriggerOnPlayerStatSelected(PlayerStatSO statSO) => OnPlayerStatSelected?.Invoke(statSO);
}