using UnityEngine;

[CreateAssetMenu(fileName = "DiceStatSO", menuName = "Scriptable Objects/DiceStatSO")]
public class DiceStatSO : ScriptableObject
{
    public DiceSpriteListSO defaultDiceSpriteListSO;
    public int defaultPlayDiceCount = 5;
    public int defaultAvailityDiceMax = 5;
    public int defaultPlayMax = 3;
    public int defaultRollMax = 3;
}
