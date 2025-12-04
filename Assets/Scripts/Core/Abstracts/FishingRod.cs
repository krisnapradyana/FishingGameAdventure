using System;
using UnityEngine;

public abstract class FishingRod : MonoBehaviour, IRod
{
    [Header("Entity Properties")]
    public int ID { get; private set; }
    [field:SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    public float Weight { get; }
    public int BuyPrice { get; }
    public int SellPrice { get; }

    [Header("Rod Parameters")]
    [field: SerializeField] public float CastDistance { get; protected set; }
    [field: SerializeField] public float ReelingSpeed { get; protected set; }
    public float Durability { get; set; }
    [field: SerializeField] public float MaxDurability { get; protected set; }
    [field: SerializeField] public Lure EqippedLure { get; private set; }
    [field: SerializeField] public bool HasCasted { get; protected set; }

    public FishingRod(string name, string description, float weigth, int buyPrice, int sellPrice, float castDistance, float reelingSped, float maxDurability)
    {
        Name = name;
        Description = description;
        Weight = weigth;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
        CastDistance = castDistance;
        ReelingSpeed = reelingSped;
        MaxDurability = maxDurability;
    }

    public virtual void Start()
    {
        ID = GetInstanceID();
        gameObject.name = Name;
        Durability = MaxDurability;
    }

    public virtual void EquipLure(Lure lure)
    {
        if (lure == null)
        {
            Debug.Log("No Lure Equipped");
        }
        EqippedLure = lure;
        // Additional logic for equipping the lure can be added here
    }

    public virtual void Cast(float castPower)
    {
        if(EqippedLure == null)
        {
            Debug.LogWarning($"{nameof(FishingRod)} '{Name}' has no lure equipped.");
            return;
        }

        if(HasCasted)
        {
            Debug.LogWarning($"{nameof(FishingRod)} '{Name}' is already casted.");
            return;
        }

        HasCasted = true;
        // Implementation for casting the fishing rod
        var finalCastDistance = CastDistance * castPower; //cast power 1.0 = 100% distance

        // Logic to handle casting the fishing line
        // Put and spawn lure at finalCastDistance

        // --- Cast logic start ---
        castPower = Mathf.Clamp01(castPower);

        // Defensive: if no cast distance configured, do nothing
        if (finalCastDistance <= 0f)
        {
            Debug.LogWarning($"{nameof(FishingRod)} '{Name}' has no cast distance configured.");
            return;
        }

        // Calculate spawn position slightly in front of the rod and a little up so it doesn't collide with the rod
        var spawnOffset = transform.forward * 0.5f + Vector3.up * 0.1f;
        var spawnPosition = transform.position + spawnOffset;

        // Determine target point: try to raycast to find water/ground; otherwise use straight forward at final distance
        Vector3 targetPosition = transform.position + transform.forward * finalCastDistance;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, finalCastDistance))
        {
            targetPosition = hit.point;
        }

        // Create a simple lure GameObject at runtime if no prefab system is available
        GameObject lure = Instantiate(EqippedLure.gameObject, spawnPosition, Quaternion.identity);
        //lure.transform.position = spawnPosition;
        
        // Remove collider triggers that might immediately collide with the rod; keep default collider for physics interactions
        var col = lure.GetComponent<Collider>();
        if (col != null)
            col.isTrigger = false;

        // Add Rigidbody to let physics handle flight and splash
        var rb = lure.GetComponent<Rigidbody>();
        rb.mass = 0.2f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Compute launch velocity to roughly reach the target; tune multiplier as needed
        var toTarget = (targetPosition - spawnPosition);
        var distance = toTarget.magnitude;
        var direction = toTarget.normalized;
        // Basic velocity estimate (not ballistic); for simple gameplay this is acceptable.
        var baseLaunchSpeed = Mathf.Max(5f, distance * 2f);
        rb.linearVelocity = direction * baseLaunchSpeed * Mathf.Lerp(0.6f, 1.2f, castPower);

        // Optional: tag the lure for other systems to find
        try
        {
            lure.tag = "Lure";
        }
        catch
        {
            // If the project doesn't have a "Lure" tag defined, ignore.
        }

        // Reduce durability on cast; non-destructive if MaxDurability not set
        if (MaxDurability > 0f)
        {
            var durabilityLoss = Mathf.Lerp(0.5f, 2.0f, castPower);
            Durability = Mathf.Max(0f, Durability - durabilityLoss);
        }
        // --- Cast logic end ---
    }

    public virtual void Reel()
    {
        if(!HasCasted)
        {
            Debug.LogWarning($"{nameof(FishingRod)} '{Name}' has not been casted yet.");
            return;
        }
        // Implementation for reeling the fishing rod
        // Logic to handle reeling in the fishing line
        //HasCasted = false; // Reset casted state after reeling in

        //Reel Instantly if no fish
    }
}
