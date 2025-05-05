using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceFaceSpriteListSO", menuName = "Scriptable Objects/DiceFaceSpriteListSO")]
public class DiceFaceSpriteListSO : ScriptableObject
{
    public List<Sprite> spriteList;
    public int DiceFaceCount => spriteList == null ? 0 : spriteList.Count;
    public Material spriteMaterial;
    public Material imageMaterial;
}
