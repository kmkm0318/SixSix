using System;
using UnityEngine;

public static class ToolTipUIEvents
{
    public static event Action<Transform, Vector2, string, string, ToolTipTag, AbilityDiceRarity> OnToolTipShowRequested;
    public static event Action OnToolTipHideRequested;

    public static void TriggerOnToolTipShowRequested(Transform transform, Vector2 direction, string name, string description, ToolTipTag tag = ToolTipTag.None, AbilityDiceRarity rarity = AbilityDiceRarity.Normal)
    => OnToolTipShowRequested?.Invoke(transform, direction, name, description, tag, rarity);
    public static void TriggerOnToolTipHideRequested() => OnToolTipHideRequested?.Invoke();
}