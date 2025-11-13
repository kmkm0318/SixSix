using System;

public static class GameResultUIEvents
{
    public static Action<bool> OnGameResultUIShowRequested;

    public static void TriggerOnGameResultUIShowRequested(bool isClear) => OnGameResultUIShowRequested?.Invoke(isClear);
}