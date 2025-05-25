using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GambleDiceListSO", menuName = "Scriptable Objects/GambleDiceListSO")]
public class GambleDiceListSO : ScriptableObject
{
    public List<GambleDiceSO> gambleDiceSOList;

    public GambleDiceSO GetRandomGambleDiceSO()
    {
        if (gambleDiceSOList == null || gambleDiceSOList.Count == 0)
        {
            Debug.LogWarning("AbilityDiceSO list is empty or null.");
            return null;
        }

        int randomIndex = Random.Range(0, gambleDiceSOList.Count);
        return gambleDiceSOList[randomIndex];
    }
}