using System;
using UnityEngine;

public abstract class Triggers : MonoBehaviour, IInteractable
{
    public Action OnInteracted { get; set; }
    public virtual void Interact()
    {
        Debug.Log($"Interacted <color=orange>{this.gameObject.name}</color>");
        OnInteracted?.Invoke();
    }
}
