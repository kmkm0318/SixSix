using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    public readonly float DefaultDuration = 0.5f;

    #region DiceStatSO
    [SerializeField] private DiceStatSO currentDiceStat;
    public DiceStatSO CurrentDiceStat => currentDiceStat;
    public DiceSpriteListSO DefaultDiceSpriteList => currentDiceStat.defaultDiceSpriteListSO;
    public int DefaultPlayDiceCount => currentDiceStat.defaultPlayDiceCount;
    public int DefaultAvailityDiceMax => currentDiceStat.defaultAvailityDiceMax;
    public int DefaultMaxPlay => currentDiceStat.defaultPlayMax;
    public int DefaultMaxRoll => currentDiceStat.defaultRollMax;
    #endregion

    #region ShaderDataSO
    [SerializeField] private ShaderDataSO defaultShaderData;
    public ShaderDataSO DefaultShaderData => defaultShaderData;
    [SerializeField] private ShaderDataSO chaosShaderData;
    public ShaderDataSO ChaosShaderData => chaosShaderData;
    #endregion

    #region AvailityDiceListSO
    [SerializeField] private AvailityDiceListSO shopAvailityDiceListSO;
    public AvailityDiceListSO ShopAvailityDiceListSO => shopAvailityDiceListSO;
    #endregion

    #region GambleDiceListSO
    [SerializeField] private GambleDiceListSO shopGambleDiceListSO;
    public GambleDiceListSO ShopGambleDiceListSO => shopGambleDiceListSO;
    #endregion

    #region HandListSO
    [SerializeField] private HandListSO totalHandListSO;
    public HandListSO TotalHandListSO => totalHandListSO;

    public HandSO GetHandSO(Hand hand)
    {
        return totalHandListSO.handList.Find(x => x.hand == hand);
    }
    #endregion

    #region LayerMask
    [SerializeField] private LayerMask playergroundLayerMask;
    public LayerMask PlayergroundLayerMask => playergroundLayerMask;
    #endregion
}
