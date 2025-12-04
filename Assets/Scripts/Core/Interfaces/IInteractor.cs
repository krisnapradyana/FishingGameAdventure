using System;
using System.Collections;
using UnityEngine;

public interface IInteractor
{
    float InteractionRange { get; }
    int MaxColliders { get; }
    Action OnInteract { get; }
    Action<Collider> OnInRange { get; }
    Action<Collider> OnExitRange { get; }
    string InteractionTag { get; }
    InputSystem_Actions Input { get; }
    Collider InteractedObject { get; }
    void Interact(Collider col);
    void ProcessOverlap();
}