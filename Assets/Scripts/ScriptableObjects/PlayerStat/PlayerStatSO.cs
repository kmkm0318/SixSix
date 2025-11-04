using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "PlayerStatSO", menuName = "Scriptable Objects/PlayerStatSO")]
public class PlayerStatSO : ScriptableObject
{
    public DiceSpriteListSO diceSpriteListSO;
    public ShaderDataSO shaderDataSO;
    public LocalizedString playerStatName;
    public LocalizedString playerStatDescription;
    public int id;
    public int price;

    public double initialTargetRoundScore = 600;

    public int playDiceCount = 5;
    public int abilityDiceMax = 5;
    public int gambleDiceMax = 3;
    public int gambleDiceSaveMax = 5;

    public int playMax = 3;
    public int rollMax = 3;

    public int startMoney = 0;
    public int bonusMoney = 25;
    public int roundClearReward = 5;
    public int playRemainReward = 2;
    public int interestUnit = 10;
    public int interestPerUnit = 1;
    public int interestMax = 10;
    public int bossRoundReward = 10;
}
