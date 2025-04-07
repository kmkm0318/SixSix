using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    [SerializeField] private DiceFaceSpriteListSO defaultDiceList;
    public DiceFaceSpriteListSO DefaultDiceList => defaultDiceList;

    [SerializeField] private AvailityDiceListSO availityDiceListSO;
    public AvailityDiceListSO AvailityDiceListSO => availityDiceListSO;

    [SerializeField] private HandCategoryListSO handCategoryListSO;
    public HandCategoryListSO HandCategoryListSO => handCategoryListSO;
    [SerializeField] private HandCategoryListSO standardHandCategoryListSO;
    public HandCategoryListSO StandardHandCategoryListSO => standardHandCategoryListSO;
    [SerializeField] private HandCategoryListSO specialHandCategoryListSO;
    public HandCategoryListSO SpecialHandCategoryListSO => specialHandCategoryListSO;

    [SerializeField] private LayerMask playergroundLayerMask;
    public LayerMask PlayergroundLayerMask => playergroundLayerMask;

    public HandCategorySO GetHandCategorySO(HandCategory handCategory)
    {
        return handCategoryListSO.handCategoryList.Find(x => x.handCategory == handCategory);
    }
}
