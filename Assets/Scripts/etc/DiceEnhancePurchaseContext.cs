public class DiceEnhancePurchaseContext
{
    public ScorePair EnhanceValue { get; private set; }
    public int Price { get; private set; }
    public int Index { get; private set; }

    public DiceEnhancePurchaseContext(ScorePair enhanceValue, int price, int index)
    {
        EnhanceValue = enhanceValue;
        Price = price;
        Index = index;
    }
}