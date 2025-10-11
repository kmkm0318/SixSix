using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatSO", menuName = "Scriptable Objects/PlayerStatSO")]
public class PlayerStatSO : ScriptableObject
{
    public DiceSpriteListSO defaultDiceSpriteListSO;

    public double defaultInitialTargetRoundScore = 600;

    public int defaultPlayDiceCount = 5;
    public int defaultAbilityDiceMax = 5;
    public int defaultGambleDiceMax = 3;
    public int defaultGambleDiceSaveMax = 5;

    public int defaultPlayMax = 3;
    public int defaultRollMax = 3;

    public int defaultStartMoney = 0;
    public int bonusMoney = 25;
    public int roundClearReward = 10;
    public int playRemainReward = 2;
    public int interestUnit = 10;
    public int interestPerUnit = 1;
    public int interestMax = 10;
    public int bossRoundReward = 10;
}
