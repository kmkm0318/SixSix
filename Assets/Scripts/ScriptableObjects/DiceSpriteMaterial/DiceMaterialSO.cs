using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceMaterialSO", menuName = "Scriptable Objects/DiceMaterialSO")]
public class DiceMaterialSO : ScriptableObject
{
    public Material defaultMaterial;
    public Material uiMaterial;
}
