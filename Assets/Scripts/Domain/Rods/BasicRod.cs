using UnityEngine;

public class BasicRod : FishingRod
{
    public BasicRod() : base(
        name: "Basic Rod",
        description: "A simple fishing rod for beginners.",
        weigth: 2.0f,
        buyPrice: 50,
        sellPrice: 25,
        castDistance: 15.0f,
        reelingSped: 1.0f,
        maxDurability: 100
        ) 
    { }

    public override void Start()
    {
        // Initialization logic if needed
        base.Start();
    }

    public override void Cast(float castPower)
    {
        base.Cast(castPower);
    }

    public override void Reel()
    {
        base.Reel();
    }
}
