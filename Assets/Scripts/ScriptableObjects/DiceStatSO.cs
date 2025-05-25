using UnityEngine;

[CreateAssetMenu(fileName = "DiceStatSO", menuName = "Scriptable Objects/DiceStatSO")]
public class DiceStatSO : ScriptableObject
{
    public DiceSpriteListSO defaultDiceSpriteListSO;
    public int defaultStartMoney = 0;
    public double defaultInitialTargetRoundScore = 600;
    public int defaultPlayDiceCount = 5;
    public int defaultAbilityDiceMax = 5;
    public int defaultGambleDiceMax = 3;
    public int defaultGambleDiceSaveMax = 5;
    public int defaultPlayMax = 3;
    public int defaultRollMax = 3;
}
