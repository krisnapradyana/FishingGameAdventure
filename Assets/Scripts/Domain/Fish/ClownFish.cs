using UnityEngine;

public class ClownFish : FishBase
{
    public ClownFish() : base(
        name: "Clownfish",
        description: "A small, colorful fish known for its symbiotic relationship with sea anemones.",
        moveSpeed: 2.5f,
        homeRadius: 5.0f,
        pullResistance: 3.0f,
        StruggleStrenght: 4.0f,
        catchDifficulty: 3.0f,
        stamina: 15.0f,
        weight: 0.5f,
        sellPrice: 20)
    { }

    public override void Start()
    {
        base.Start();
        base.SetSwimAnchor(Vector3.zero);
        base.SetPlayer(GameObject.FindFirstObjectByType<Player>());
    }
}

