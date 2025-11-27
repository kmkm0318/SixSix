using System;
using System.Collections.Generic;

/// <summary>
/// 핸드 성공 관련 UI 이벤트들
/// </summary>
public static class HandSuccessUIEvents
{
    /// <summary>
    /// 핸드 성공 여부 업데이트 요청 이벤트
    /// </summary>
    public static event Action<Dictionary<Hand, ScorePair>> OnHandSuccessed;

    /// <summary>
    /// 핸드 성공 여부 업데이트 요청 트리거 함수
    /// </summary>
    public static void TriggerOnHandSuccessed(Dictionary<Hand, ScorePair> handCompletions) => OnHandSuccessed?.Invoke(handCompletions);
}