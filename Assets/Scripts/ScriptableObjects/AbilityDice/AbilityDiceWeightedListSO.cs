using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceWeightedListSO", menuName = "Scriptable Objects/AbilityDiceWeightedListSO", order = 0)]
public class AbilityDiceWeightedListSO : ScriptableObject
{
    public List<WeightedItem<AbilityDiceListSO>> diceLists;
}