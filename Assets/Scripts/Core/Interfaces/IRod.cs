public interface IRod : IEntity, IInventoryItem
{
    float CastDistance { get; }
    float ReelingSpeed { get; }
    float Durability { get; set; }
    float MaxDurability { get; }
    bool HasCasted { get; }
}