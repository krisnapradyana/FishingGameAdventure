using UnityEngine;

public class SharkFish : FishBase
{
    public SharkFish() : base(
        name: "Shark",
        description: "A large and powerful predator known for its sharp teeth and keen senses.",
        moveSpeed: 6.0f,
        homeRadius: 10.0f,
        pullResistance: 8.0f,
        StruggleStrenght: 9.0f,
        catchDifficulty: 9.0f,
        stamina: 30.0f,
        weight: 200.0f,
        sellPrice: 500)
    { }
    public override void Start()
    {
        base.Start();
        base.SetSwimAnchor(Vector3.zero);
        base.SetPlayer(GameObject.FindFirstObjectByType<Player>());
    }
}
