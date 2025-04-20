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

    #region HandCategoryListSO
    [SerializeField] private HandCategoryListSO handCategoryListSO;
    public HandCategoryListSO HandCategoryListSO => handCategoryListSO;
    [SerializeField] private HandCategoryListSO standardHandCategoryListSO;
    public HandCategoryListSO StandardHandCategoryListSO => standardHandCategoryListSO;
    [SerializeField] private HandCategoryListSO specialHandCategoryListSO;
    public HandCategoryListSO SpecialHandCategoryListSO => specialHandCategoryListSO;

    public HandCategorySO GetHandCategorySO(HandCategory handCategory)
    {
        return handCategoryListSO.handCategoryList.Find(x => x.handCategory == handCategory);
    }
    #endregion

    #region LayerMask
    [SerializeField] private LayerMask playergroundLayerMask;
    public LayerMask PlayergroundLayerMask => playergroundLayerMask;
    #endregion
}
