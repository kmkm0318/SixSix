using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    #region DiceFaceSpriteListSO
    [SerializeField] private DiceFaceSpriteListSO defaultDiceList;
    public DiceFaceSpriteListSO DefaultDiceList => defaultDiceList;
    #endregion

    #region AvailityDiceListSO
    [SerializeField] private AvailityDiceListSO availityDiceListSO;
    public AvailityDiceListSO AvailityDiceListSO => availityDiceListSO;
    [SerializeField] private AvailityDiceListSO merchantAvailityDiceListSO;
    public AvailityDiceListSO MerchantAvailityDiceListSO => merchantAvailityDiceListSO;
    #endregion

    #region HandListSO
    [SerializeField] private HandListSO totalHandListSO;
    public HandListSO TotalHandListSO => totalHandListSO;
    [SerializeField] private HandListSO standardHandListSO;
    public HandListSO StandardHandListSO => standardHandListSO;
    [SerializeField] private HandListSO specialHandListSO;
    public HandListSO SpecialHandListSO => specialHandListSO;

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
