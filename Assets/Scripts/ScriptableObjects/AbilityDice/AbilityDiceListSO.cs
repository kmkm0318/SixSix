using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceListSO", menuName = "Scriptable Objects/AbilityDiceListSO")]
public class AbilityDiceListSO : ScriptableObject
{
    public List<AbilityDiceSO> abilityDiceSOList;

    public AbilityDiceSO GetRandomAbilityDiceSO()
    {
        if (abilityDiceSOList == null || abilityDiceSOList.Count == 0)
        {
            Debug.LogWarning("AbilityDiceSO list is empty or null.");
            return null;
        }

        int randomIndex = Random.Range(0, abilityDiceSOList.Count);
        return abilityDiceSOList[randomIndex];
    }
}