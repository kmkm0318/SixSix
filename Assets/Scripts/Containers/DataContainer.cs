using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{

    #region DiceStatSO
    [SerializeField] private DiceStatSO currentDiceStat;
    public DiceStatSO CurrentDiceStat => currentDiceStat;
    public DiceSpriteListSO DefaultDiceSpriteList => currentDiceStat.defaultDiceSpriteListSO;
    #endregion

    #region ShaderDataSO
    [SerializeField] private ShaderDataSO defaultShaderData;
    public ShaderDataSO DefaultShaderData => defaultShaderData;
    [SerializeField] private ShaderDataSO chaosShaderData;
    public ShaderDataSO ChaosShaderData => chaosShaderData;
    #endregion

    #region AbilityDiceListSO
    [SerializeField] private AbilityDiceListSO shopAbilityDiceListSO;
    public AbilityDiceListSO ShopAbilityDiceListSO => shopAbilityDiceListSO;
    #endregion

    #region GambleDiceListSO
    [SerializeField] private GambleDiceListSO shopGambleDiceListSO;
    public GambleDiceListSO ShopGambleDiceListSO => shopGambleDiceListSO;
    [SerializeField] private GambleDiceListSO bossGambleDiceListSO;
    public GambleDiceListSO BossGambleDiceListSO => bossGambleDiceListSO;
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

    #region DefaultColorSO
    [SerializeField] private DefaultColorSO defaultColorSO;
    public DefaultColorSO DefaultColorSO => defaultColorSO;
    #endregion
}
