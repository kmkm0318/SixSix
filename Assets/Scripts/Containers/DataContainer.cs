using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    #region DiceFaceSpriteListSO
    [SerializeField] private DiceFaceSpriteListSO defaultDiceList;
    public DiceFaceSpriteListSO DefaultDiceList => defaultDiceList;
    [SerializeField] private DiceFaceSpriteListSO playDiceList;
    public DiceFaceSpriteListSO PlayDiceList => playDiceList;
    [SerializeField] private DiceFaceSpriteListSO chaosDiceList;
    public DiceFaceSpriteListSO ChaosDiceList => chaosDiceList;
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
