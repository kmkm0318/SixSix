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
    [SerializeField] private HandListSO oneDiceHandListSO;
    public HandListSO OneDiceHandListSO => oneDiceHandListSO;
    [SerializeField] private HandListSO fourDiceHandListSO;
    public HandListSO FourDiceHandListSO => fourDiceHandListSO;
    [SerializeField] private HandListSO fiveDiceHandListSO;
    public HandListSO FiveDiceHandListSO => fiveDiceHandListSO;
    [SerializeField] private HandListSO sixDiceHandListSO;
    public HandListSO SixDiceHandListSO => sixDiceHandListSO;

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
