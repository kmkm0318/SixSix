using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandListSO", menuName = "Scriptable Objects/HandListSO")]
public class HandListSO : ScriptableObject
{
    public List<HandSO> handList;

    public HandSO GetRandomHandSO()
    {
        if (handList.Count == 0)
        {
            Debug.LogError("HandList is empty!");
            return null;
        }

        int randomIndex = Random.Range(0, handList.Count);
        return handList[randomIndex];
    }
}