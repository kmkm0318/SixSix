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
    public int DefaultMaxPlay => currentDiceStat.defaultMaxPlay;
    public int DefaultMaxRoll => currentDiceStat.defaultMaxRoll;
    #endregion

    #region DiceMaterialSO
    [SerializeField] private DiceMaterialSO defaultDiceMaterial;
    public DiceMaterialSO DefaultDiceMaterial => defaultDiceMaterial;
    [SerializeField] private DiceMaterialSO chaosDiceMaterial;
    public DiceMaterialSO ChaosDiceMaterial => chaosDiceMaterial;
    #endregion

    #region AvailityDiceListSO
    [SerializeField] private AvailityDiceListSO shopAvailityDiceListSO;
    public AvailityDiceListSO ShopAvailityDiceListSO => shopAvailityDiceListSO;
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
