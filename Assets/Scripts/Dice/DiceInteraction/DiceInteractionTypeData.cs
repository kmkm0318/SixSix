using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// 다이스 상호작용 타입 데이터 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "DiceInteractionTypeData", menuName = "Scriptable Objects/Dice/DiceInteractionTypeData", order = 0)]
public class DiceInteractionTypeData : ScriptableObject
{
    public DiceInteractionType type;
    public Color color;
    public LocalizedString localizedText;
}