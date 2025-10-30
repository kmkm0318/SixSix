using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GambleDiceListSO", menuName = "Scriptable Objects/GambleDiceListSO")]
public class GambleDiceListSO : ScriptableObject
{
    public List<GambleDiceSO> gambleDiceSOList;
}