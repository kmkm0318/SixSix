using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailityDiceListSO", menuName = "Scriptable Objects/AvailityDiceListSO")]
public class AvailityDiceListSO : ScriptableObject
{
    public List<AvailityDiceSO> availityDiceSOList;

    public AvailityDiceSO GetRandomAvailityDiceSO()
    {
        if (availityDiceSOList == null || availityDiceSOList.Count == 0)
        {
            Debug.LogWarning("AvailityDiceSO list is empty or null.");
            return null;
        }

        int randomIndex = Random.Range(0, availityDiceSOList.Count);
        return availityDiceSOList[randomIndex];
    }
}