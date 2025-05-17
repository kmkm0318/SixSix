using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSpriteListSO", menuName = "Scriptable Objects/DiceSpriteListSO")]
public class DiceSpriteListSO : ScriptableObject
{
    public List<Sprite> spriteList;
    public int DiceFaceCount => spriteList == null ? 0 : spriteList.Count;
}
