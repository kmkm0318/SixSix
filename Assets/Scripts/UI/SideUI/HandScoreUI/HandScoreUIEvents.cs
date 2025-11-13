using System;
using System.Collections.Generic;

public static class HandScoreUIEvents
{
    public static event Action<Dictionary<Hand, ScorePair>> OnHandScoreUIUpdateRequested;
    public static event Action OnHandScoreUIResetRequested;
    public static event Action<Hand, int, ScorePair> OnHandScoreUIPlayHandTriggerAnimationRequested;

    public static void TriggerOnHandScoreUIUpdateRequested(Dictionary<Hand, ScorePair> handScores) => OnHandScoreUIUpdateRequested?.Invoke(handScores);
    public static void TriggerOnHandScoreUIResetRequested() => OnHandScoreUIResetRequested?.Invoke();
    public static void TriggerOnHandScoreUIPlayHandTriggerAnimationRequested(Hand hand, int enhanceLevel, ScorePair scorePair) => OnHandScoreUIPlayHandTriggerAnimationRequested?.Invoke(hand, enhanceLevel, scorePair);
}