using System;

[Serializable]
public struct WeightedItem<Item>
{
    public Item item;
    public int weight;
}