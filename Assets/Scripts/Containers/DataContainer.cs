using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    #region DiceSpriteListSO
    [SerializeField] private DiceSpriteListSO defaultDiceList;
    public DiceSpriteListSO DefaultDiceList => defaultDiceList;
    [SerializeField] private DiceSpriteListSO numberDiceList;
    public DiceSpriteListSO NumberDiceList => numberDiceList;
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
