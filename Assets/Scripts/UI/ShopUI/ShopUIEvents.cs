using System;

public static class ShopUIEvents
{
    public static Action OnShopUIShowRequested;
    public static Action OnShopUIShown;
    public static Action OnShopUIHidden;

    public static void TriggerOnShopUIShowRequested() => OnShopUIShowRequested?.Invoke();
    public static void TriggerOnShopUIShown() => OnShopUIShown?.Invoke();
    public static void TriggerOnShopUIHidden() => OnShopUIHidden?.Invoke();
}