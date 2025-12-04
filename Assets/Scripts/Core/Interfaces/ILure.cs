using System;

public interface ILure : IEntity, IInventoryItem
{
    float lureEffectiveness { get; }
    float lureAttractRadius { get; }
    Action OnHittingWater { get; set; }
}
