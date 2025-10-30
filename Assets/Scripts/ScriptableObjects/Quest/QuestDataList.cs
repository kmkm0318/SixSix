using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDataList", menuName = "Scriptable Objects/Quest/QuestDataList", order = 0)]
public class QuestDataList : ScriptableObject
{
    public List<QuestData> questDatas;
}