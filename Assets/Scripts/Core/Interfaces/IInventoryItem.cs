public interface IInventoryItem
{
    string Name { get; }
    float Weight { get; }
    int BuyPrice { get; }
    int SellPrice { get; }
}