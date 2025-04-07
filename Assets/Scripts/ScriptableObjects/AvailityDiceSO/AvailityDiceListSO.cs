using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailityDiceListSO", menuName = "Scriptable Objects/AvailityDiceListSO")]
public class AvailityDiceListSO : ScriptableObject
{
    public List<AvailityDiceSO> availityDiceSOList;
}