using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceFaceSpriteListSO", menuName = "Scriptable Objects/DiceFaceSpriteListSO")]
public class DiceFaceSpriteListSO : ScriptableObject
{
    public List<DiceFaceSpriteSO> diceFaceList;
}
