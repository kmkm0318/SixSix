using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatSO", menuName = "Scriptable Objects/PlayerStatSO")]
public class PlayerStatSO : ScriptableObject
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
