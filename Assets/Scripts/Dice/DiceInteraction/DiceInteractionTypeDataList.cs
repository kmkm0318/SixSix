using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다이스 상호작용 타입 데이터 리스트 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "DiceInteractionTypeDataList", menuName = "Scriptable Objects/Dice/DiceInteractionTypeDataList", order = 0)]
public class DiceInteractionTypeDataList : ScriptableObject
{
    public List<DiceInteractionTypeData> DataList;

    private Dictionary<DiceInteractionType, DiceInteractionTypeData> _dataDict;
    public Dictionary<DiceInteractionType, DiceInteractionTypeData> DataDict
    {
        get
        {
            if (_dataDict == null)
            {
                _dataDict = new Dictionary<DiceInteractionType, DiceInteractionTypeData>();
                foreach (var data in DataList)
                {
                    _dataDict[data.type] = data;
                }
            }
            return _dataDict;
        }
    }
}