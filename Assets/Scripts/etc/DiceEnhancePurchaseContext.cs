public class DiceEnhancePurchaseContext
{
    public int EnhanceLevel { get; private set; }
    public ScorePair EnhanceValue { get; private set; }
    public int Price { get; private set; }
    public int Index { get; private set; }

    public DiceEnhancePurchaseContext(int enhanceLevel, ScorePair enhanceValue, int price, int index)
    {
        EnhanceLevel = enhanceLevel;
        EnhanceValue = enhanceValue;
        Price = price;
        Index = index;
    }
}