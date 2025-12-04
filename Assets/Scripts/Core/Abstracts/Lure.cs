using System;
using UnityEngine;

public abstract class Lure : MonoBehaviour, ILure
{
    //Entity Properties
    [Header("Entity Properties")]
    public int ID { get; protected set; }

    [field: SerializeField] public string Name { get; protected set; }

    [field: SerializeField] public string Description { get; protected set; }

    //Lure Properties
    [Header("Lure Properties")]
    [field: SerializeField] public float lureEffectiveness { get; protected set; }

    [field: SerializeField] public float lureAttractRadius { get; protected set; }

    //interface IInventoryItem Properties
    [Header("Invetory Properties")]
    [field: SerializeField] public float Weight { get; protected set; }

    [field: SerializeField] public int BuyPrice { get; protected set; }

    [field: SerializeField] public int SellPrice { get; protected set; }
    public Action OnHittingWater { get; set; }

    //Physics Relater
    Rigidbody _rigidbody;

    public Lure (string name, string description, float effectiveness, float attractRadius, float weight, int buyPrice, int sellPrice)
    {
        Name = name;
        Description = description;
        lureEffectiveness = effectiveness;
        lureAttractRadius = attractRadius;
        Weight = weight;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
    }

    public virtual void Start()
    {
        ID = GetInstanceID();
        gameObject.name = Name;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            Debug.Log("Hitting Water");
            _rigidbody.isKinematic = true;
            OnHittingWater.Invoke();
        }
        else
        {
            Debug.Log("Not Hitting Water");
        }
    }

    public virtual void BobberMotion()
    {

    }
}
