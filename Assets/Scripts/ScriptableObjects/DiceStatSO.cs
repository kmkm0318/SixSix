using UnityEngine;

[CreateAssetMenu(fileName = "DiceStatSO", menuName = "Scriptable Objects/DiceStatSO")]
public class DiceStatSO : ScriptableObject
{
    public DiceSpriteListSO defaultDiceSpriteListSO;
    public int defaultPlayDiceCount = 5;
    public int defaultAvailityDiceMax = 5;
    public int defaultMaxPlay = 3;
    public int defaultMaxRoll = 3;
}
