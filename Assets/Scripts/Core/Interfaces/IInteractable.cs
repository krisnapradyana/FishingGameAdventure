using System;
public interface IInteractable
{
    Action OnInteracted { get; set; }
    void Interact();
}