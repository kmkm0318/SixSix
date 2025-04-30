public class HandEnhancePurchaseContext
{
    public int EnhanceLevel { get; private set; }
    public int Price { get; private set; }
    public int Index { get; private set; }

    public HandEnhancePurchaseContext(int enhanceLevel, int price, int index)
    {
        EnhanceLevel = enhanceLevel;
        Price = price;
        Index = index;
    }
}