using UnityEngine;

public class CrumpledAnts : Lure
{
    public CrumpledAnts() : base(
        name: "Crumpled Ants",
        description: "A handful of crumpled ants. Surprisingly effective for catching small fish.",
        weight: 0.05f,
        attractRadius: 1.5f,
        effectiveness : 0.3f,
        buyPrice: 2,
        sellPrice: 1
    )
    { }

    public override void Start()
    {
        base.Start();
        OnHittingWater += OnLureHitWater;
    }

    void OnLureHitWater()
    {
        Debug.Log($"{Name} has hit the water and is now active.");
        // Additional logic specific to Crumpled Ants when hitting water can be added here
    }
}
