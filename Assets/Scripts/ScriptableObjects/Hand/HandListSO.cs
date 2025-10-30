using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandListSO", menuName = "Scriptable Objects/HandListSO")]
public class HandListSO : ScriptableObject
{
    public List<HandSO> handList;
}