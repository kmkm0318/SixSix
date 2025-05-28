public class EnhancePurchaseContext
{
    public EnhanceType EnhanceType { get; private set; }
    public int EnhanceLevel { get; private set; }
    public ScorePair EnhanceValue { get; private set; }
    public int Price { get; private set; }
    public int Index { get; private set; }

    public EnhancePurchaseContext(EnhanceType enhanceType, int enhanceLevel, ScorePair enhanceValue, int price, int index)
    {
        EnhanceType = enhanceType;
        EnhanceLevel = enhanceLevel;
        EnhanceValue = enhanceValue;
        Price = price;
        Index = index;
    }
}