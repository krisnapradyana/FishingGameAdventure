using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerInteractor : Interactor
{
    public Object interactedObject;
    new private void OnDestroy()
    {
        base.OnDestroy();
    }   

    new private void Start()
    {
        base.Start();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Interact(Collider col)
    {
        try
        {
            interactedObject = col.gameObject;
            IInteractable interactable = interactedObject.GetComponent<IInteractable>();
            interactable.OnInteracted += () => Debug.Log($"Player interacted with <color=magenta>{interactedObject.name}</color>");
            interactable.Interact();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Error during interaction: {e.Message}");
        }
    }

    public override void ExitRange(Collider other)
    {
        base.ExitRange(other);
        interactedObject.GetComponent<IInteractable>().OnInteracted = null;
    }
}
