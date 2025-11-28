using System;
using UnityEngine;

/// <summary>
/// 파티클 이벤트 관리 클래스
/// </summary>
public static class ParticleEvents
{
    // 주사위 충돌 이벤트
    public static event Action<Vector3, Vector3> OnDiceCollide;
    // 핸드 성공 이벤트
    public static event Action<Vector3> OnHandSuccess;

    // 주사위 충돌 이벤트 트리거
    public static void TriggerOnDiceCollide(Vector3 pos, Vector3 dir) => OnDiceCollide?.Invoke(pos, dir);
    // 핸드 성공 이벤트 트리거
    public static void TriggerOnHandSuccess(Vector3 pos) => OnHandSuccess?.Invoke(pos);
}