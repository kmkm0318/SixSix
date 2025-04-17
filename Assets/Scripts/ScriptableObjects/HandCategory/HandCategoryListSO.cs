using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandCategoryListSO", menuName = "Scriptable Objects/HandCategoryListSO")]
public class HandCategoryListSO : ScriptableObject
{
    public List<HandCategorySO> handCategoryList;

    public HandCategorySO GetRandomHandCategorySO()
    {
        if (handCategoryList.Count == 0)
        {
            Debug.LogError("HandCategoryList is empty!");
            return null;
        }

        int randomIndex = Random.Range(0, handCategoryList.Count);
        return handCategoryList[randomIndex];
    }
}