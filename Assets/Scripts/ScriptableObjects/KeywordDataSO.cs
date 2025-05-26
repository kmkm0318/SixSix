using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "KeywordDataSO", menuName = "Scriptable Objects/KeywordDataSO")]
public class KeywordDataSO : ScriptableObject
{
    public string keyword;
    public Sprite sprite;
    public LocalizedString localizedName;
    public Color color;
}