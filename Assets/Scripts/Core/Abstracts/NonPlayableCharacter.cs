using System;
using UnityEngine;

public abstract class NonPlayableCharacter : MonoBehaviour, IMoveable, IInteractable, IEntity
{
    public int ID { get; protected set; }
    [field:SerializeField]public string Name { get; protected set; }
    [field:SerializeField]public string Description { get; protected set; }

    [Header("NPC Parameters")]
    [field:SerializeField] public float MoveSpeed { get; set; } = 5f;
    public Action OnInteracted { get; set; }
    public NonPlayableCharacter(string name, string description)
    {
        Name = name;
        Description = description;
    }

    protected virtual void Start()
    {
        ID = GetInstanceID();
    }

    public void Interact()
    {
        Debug.Log("NPC being Interacted");
        OnInteracted?.Invoke();
    }

    public void Jump()
    {
        throw new NotImplementedException();
    }

    public void Move(Vector2 direction)
    {
        Debug.Log("NPC Moving");
    }
}
