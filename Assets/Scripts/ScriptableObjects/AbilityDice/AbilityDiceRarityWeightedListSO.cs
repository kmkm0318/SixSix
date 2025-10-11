using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceRarityWeightedListSO", menuName = "Scriptable Objects/AbilityDiceRarityWeightedListSO", order = 0)]
public class AbilityDiceRarityWeightedListSO : ScriptableObject
{
    public List<WeightedItem<AbilityDiceRarity>> rarityList;
}