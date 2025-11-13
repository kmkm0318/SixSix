using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    #region PlayerStatSO
    [SerializeField] private PlayerStatListSO playerStatListSO;
    public PlayerStatListSO PlayerStatListSO => playerStatListSO;
    [SerializeField] private PlayerStatSO currentPlayerStat;
    public PlayerStatSO CurrentPlayerStat
    {
        get => currentPlayerStat;
        set => currentPlayerStat = value;
    }
    public DiceSpriteListSO DefaultDiceSpriteList => CurrentPlayerStat.diceSpriteListSO;
    #endregion

    #region DiceShaderDataSO
    [SerializeField] private ShaderDataSO playDiceShaderData;
    public ShaderDataSO PlayDiceShaderData => playDiceShaderData;
    [SerializeField] private ShaderDataSO chaosDiceShaderData;
    public ShaderDataSO ChaosDiceShaderData => chaosDiceShaderData;
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
