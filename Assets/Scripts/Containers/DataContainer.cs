using UnityEngine;

public class DataContainer : Singleton<DataContainer>
{
    [SerializeField] private DiceFaceSpriteListSO defaultDiceList;
    public DiceFaceSpriteListSO DefaultDiceList => defaultDiceList;
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
}
