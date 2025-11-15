using System;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 다이스 오브젝트 및 갬블 다이스 아이콘의 상호작용을 전달하기 위한 이벤트
/// </summary>
public static class InteractionInfoUIEvents
{
    public static event Action<Transform, DiceInteractionType, int> OnShowInteractionInfoUI;
    public static event Action<Transform> OnHideInteractionInfoUI;

    public static void TriggerOnShowInteractionInfoUI(Transform target, DiceInteractionType interactType, int value = 0)
    => OnShowInteractionInfoUI?.Invoke(target, interactType, value);
    public static void TriggerOnHideInteractionInfoUI(Transform target)
    => OnHideInteractionInfoUI?.Invoke(target);
}