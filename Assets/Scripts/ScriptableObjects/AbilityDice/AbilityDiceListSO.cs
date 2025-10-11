using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceListSO", menuName = "Scriptable Objects/AbilityDiceListSO")]
public class AbilityDiceListSO : ScriptableObject
{
    public List<AbilityDiceSO> abilityDiceSOList;
}