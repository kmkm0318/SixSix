using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    #region PlayerStatSO
    [SerializeField] private PlayerStatSO currentPlayerStat;
    public PlayerStatSO CurrentPlayerStat => currentPlayerStat;
    public DiceSpriteListSO DefaultDiceSpriteList => currentPlayerStat.defaultDiceSpriteListSO;
    #endregion

    #region ShaderDataSO
    [SerializeField] private ShaderDataSO defaultShaderData;
    public ShaderDataSO DefaultShaderData => defaultShaderData;
    [SerializeField] private ShaderDataSO chaosShaderData;
    public ShaderDataSO ChaosShaderData => chaosShaderData;
    #endregion

    #region AbilityDiceListSO
    [SerializeField] private AbilityDiceListSO normalAbilityDiceListSO;
    [SerializeField] private AbilityDiceListSO rareAbilityDiceListSO;
    [SerializeField] private AbilityDiceListSO epicAbilityDiceListSO;
    [SerializeField] private AbilityDiceListSO legendaryAbilityDiceListSO;
    [SerializeField] private AbilityDiceRarityWeightedListSO abilityDiceRarityWeightedListSO;
    public AbilityDiceListSO NormalAbilityDiceListSO => normalAbilityDiceListSO;
    public AbilityDiceListSO RareAbilityDiceListSO => rareAbilityDiceListSO;
    public AbilityDiceListSO EpicAbilityDiceListSO => epicAbilityDiceListSO;
    public AbilityDiceListSO LegendaryAbilityDiceListSO => legendaryAbilityDiceListSO;
    public AbilityDiceRarityWeightedListSO AbilityDiceRarityWeightedListSO => abilityDiceRarityWeightedListSO;
    #endregion

    #region GambleDiceListSO
    [SerializeField] private GambleDiceListSO normalGambleDiceListSO;
    public GambleDiceListSO NormalGambleDiceListSO => normalGambleDiceListSO;
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
