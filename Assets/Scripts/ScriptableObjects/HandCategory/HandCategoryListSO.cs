using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandCategoryListSO", menuName = "Scriptable Objects/HandCategoryListSO")]
public class HandCategoryListSO : ScriptableObject
{
    public List<HandCategorySO> handCategoryList;
}