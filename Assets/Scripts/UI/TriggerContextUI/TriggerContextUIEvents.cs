using System;
using UnityEngine;

public static class TriggerContextUIEvents
{
    public static event Action<Transform, Vector3, ScorePair> OnShowScoreContext;
    public static event Action<Transform, Vector3, int> OnShowMoneyContext;
    public static event Action<Transform, Vector3, int, string> ShowValueContext;
    public static event Action<Transform, Vector3, string> ShowRetriggerContext;

    public static void TriggerOnShowScoreContext(Transform targetTransform, Vector3 offset, ScorePair scorePair)
    => OnShowScoreContext?.Invoke(targetTransform, offset, scorePair);
    public static void TriggerOnShowMoneyContext(Transform targetTransform, Vector3 offset, int money)
    => OnShowMoneyContext?.Invoke(targetTransform, offset, money);
    public static void TriggerOnShowValueContext(Transform targetTransform, Vector3 offset, int value, string color)
    => ShowValueContext?.Invoke(targetTransform, offset, value, color);
    public static void TriggerOnShowRetriggerContext(Transform targetTransform, Vector3 offset, string color)
    => ShowRetriggerContext?.Invoke(targetTransform, offset, color);
}